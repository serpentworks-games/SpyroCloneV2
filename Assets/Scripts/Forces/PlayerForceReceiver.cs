using ScalePact.Combat;
using ScalePact.Core.Input;
using ScalePact.Core.Player;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Forces
{
    public class PlayerForceReceiver : ForceReceiver
    {
        [Header("Grounded Movement")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float movementRotationSpeed = 20f;
        [SerializeField] float faceTargetRotationSpeed = 30f;

        [Header("Jump/Glide Movement")]
        [SerializeField] float jumpPower = 5f;
        [SerializeField] float jumpFallOff = 2f;
        [SerializeField] float lowJumpMulti = 1.5f;
        [SerializeField] float glideSpeed = 7f;
        [SerializeField] float movingGlideForce = 200f;
        [SerializeField] float stationaryGlideForce = 100f;
        [SerializeField] float glideFallOff = 0.5f;

        [Header("Slope Tolerances")]
        [SerializeField] float slopeLimit = 45f;
        [SerializeField] float slopeInfluence = 5f;

        [Header("Finding the Ground")]
        [SerializeField] float floorOffsetY = 0.05f;
        [SerializeField] float floorRaycastLength = 1f;
        [SerializeField] float raycastWidthX = 0.5f;
        [SerializeField] float raycastWidthZ = 2f;

        Camera mainCamera;
        Rigidbody rb;
        Animator animator;
        InputManager inputManager;
        PlayerCombat combat;
        TargetScanner targetScanner;

        Vector3 moveDir;
        Vector3 raycastFloorPos;
        Vector3 floorMovement;
        Vector3 gravity;
        Vector3 combinedFloorRaycast;

        float slopeAmount;
        Vector3 floorNormal;

        bool isTargetting;
        bool isInGlide;
        bool isInJump;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            inputManager = GetComponent<InputManager>();
            combat = GetComponent<PlayerCombat>();
            targetScanner = GetComponent<TargetScanner>();

            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            inputManager.JumpEvent += OnJumpPressed;
            inputManager.JumpEvent += OnGlidePressed;
            inputManager.ToggleTargetEvent += OnLockOnTarget;
        }

        private void OnDisable()
        {
            inputManager.JumpEvent -= OnJumpPressed;
            inputManager.JumpEvent -= OnGlidePressed;
            inputManager.ToggleTargetEvent -= OnLockOnTarget;
        }

        private void Update()
        {
            if (combat.IsAttacking)
            {
                rb.velocity = Vector3.zero;
                moveDir = Vector3.zero;
            }
            else
            {
                CalculateForwardMovement();
            }

            if (isInGlide && IsGrounded())
            {
                animator.SetTrigger(PlayerHashIDs.LandTriggerHash);
                animator.ResetTrigger(PlayerHashIDs.GlideTriggerHash);
                isInGlide = false;
            }

            ApplyImpact();

        }

        private void FixedUpdate()
        {
            CalculateGravity();

            CalculateRotation(moveDir, movementRotationSpeed);

            rb.velocity = CalculateVelocity() + gravity;

            AddGlideForce(CalculateVelocity());

            floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

            if (floorMovement != rb.position && IsGrounded() && rb.velocity.y <= 0)
            {
                if (isInJump)
                {
                    animator.SetTrigger(PlayerHashIDs.LandTriggerHash);
                    animator.ResetTrigger(PlayerHashIDs.JumpTriggerHash);
                    isInJump = false;
                }
                rb.MovePosition(floorMovement);
                gravity.y = 0;
            }
        }

        public override void AddForce(Vector3 forceToAdd)
        {
            impact += forceToAdd;
        }

        public void FaceTarget(Transform target)
        {
            CalculateRotation(target.position - transform.position, faceTargetRotationSpeed);
        }

        void AddGlideForce(Vector3 velocity)
        {
            if (isInGlide)
            {
                if (moveDir == Vector3.zero)
                {
                    rb.AddForce(glideSpeed * stationaryGlideForce * Time.deltaTime * velocity);
                }
                else
                {
                    rb.AddForce(glideSpeed * movingGlideForce * Time.deltaTime * velocity);
                }

            }
        }


        private void ApplyImpact()
        {
            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, impactDrag);

            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                animator.ResetTrigger(PlayerHashIDs.ImpactTriggerHash);
            }

            if (impact != Vector3.zero && combat.IsAttacking == false)
            {
                animator.SetTrigger(PlayerHashIDs.ImpactTriggerHash);
            }

            rb.AddForce(impact, ForceMode.Impulse);
        }


        #region Movement Calculations
        void CalculateForwardMovement()
        {
            Vector3 movement = new()
            {
                x = inputManager.MovementVector.x,
                y = 0,
                z = inputManager.MovementVector.y
            };

            Vector3 adjustedMoveX = movement.x * mainCamera.transform.right;
            Vector3 adjustedMoveZ = movement.z * mainCamera.transform.forward;

            Vector3 combinedInput = adjustedMoveX + adjustedMoveZ;

            moveDir = new Vector3(combinedInput.normalized.x, 0, combinedInput.normalized.z);
        }

        void CalculateRotation(Vector3 targetDirNormal, float rotationSpeed)
        {
            Vector3 targetForward = targetDirNormal;
            if (targetDirNormal == Vector3.zero)
            {
                targetForward = transform.forward;
            }
            targetForward.y = 0;

            Quaternion rot = Quaternion.LookRotation(targetForward);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * rotationSpeed);
            transform.rotation = targetRot;
        }

        private Vector3 CalculateVelocity()
        {
            if (isInGlide)
            {
                if (moveDir == Vector3.zero)
                {
                    return transform.forward * glideSpeed;
                }
                else
                {
                    return moveDir * glideSpeed;
                }
            }
            else
            {
                return moveDir * GetMoveSpeed();
            }
        }

        private void CalculateGravity()
        {
            if (!IsGrounded() || slopeAmount >= 0.1f)
            {
                if (isInGlide)
                {
                    gravity += (glideFallOff - 1) * Physics.gravity.y * Time.fixedDeltaTime * Vector3.up;
                }
                gravity += (jumpFallOff - 1) * Physics.gravity.y * Time.fixedDeltaTime * Vector3.up;
            }
        }

        float GetMoveSpeed()
        {
            return Mathf.Clamp(moveSpeed + (slopeAmount * slopeInfluence), 0, moveSpeed + 1);
        }
        #endregion

        #region Ground Checks
        bool IsGrounded()
        {
            if (GetFloorRaycasts(0, 0, 0.6f) != Vector3.zero)
            {
                slopeAmount = Vector3.Dot(transform.forward, floorNormal);
                return true;
            }

            return false;
        }

        Vector3 FindFloor()
        {
            int floorAverage = 1;

            combinedFloorRaycast = GetFloorRaycasts(0, 0, floorRaycastLength); //Get the center first
            floorAverage +=
                GetFloorAverage(raycastWidthX, 0) + GetFloorAverage(-raycastWidthX, 0)
                    + GetFloorAverage(0, raycastWidthZ) + GetFloorAverage(0, -raycastWidthZ);

            return combinedFloorRaycast / floorAverage;
        }

        Vector3 GetFloorRaycasts(float offsetX, float offsetZ, float raycastLength)
        {
            raycastFloorPos = transform.TransformPoint(0 + offsetX, 0 + 0.5f, 0 + offsetZ); //y should be height

            Debug.DrawRay(raycastFloorPos, Vector3.down * raycastLength, Color.magenta);

            if (Physics.Raycast(raycastFloorPos, -Vector3.up, out RaycastHit hit, raycastLength))
            {
                floorNormal = hit.normal;
                if (Vector3.Angle(floorNormal, Vector3.up) < slopeLimit)
                {
                    return hit.point;
                }
                else
                {
                    return Vector3.zero;
                }
            }
            else
            {
                return Vector3.zero;
            }
        }

        int GetFloorAverage(float offsetX, float offsetZ)
        {
            if (GetFloorRaycasts(offsetX, offsetZ, floorRaycastLength) != Vector3.zero)
            {
                combinedFloorRaycast += GetFloorRaycasts(offsetX, offsetZ, floorRaycastLength);
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Event Handlers
        void OnJumpPressed()
        {
            if (IsGrounded())
            {
                animator.SetTrigger(PlayerHashIDs.JumpTriggerHash);
            }
        }

        void OnGlidePressed()
        {
            if (!IsGrounded())
            {
                isInGlide = true;
                animator.SetTrigger(PlayerHashIDs.GlideTriggerHash);
            }
        }

        void OnLockOnTarget()
        {
            if (targetScanner.TargetColliders.Count < 0)
            {
                isTargetting = false;
                //change control type
                //set animation bool
                return;
            }
            isTargetting = !isTargetting;

            //set anim bool

            if (isTargetting)
            {
                //change to locked on movement
            }
            else
            {
                //change to normal movement
            }
        }
        #endregion

        #region Anim Events
        void ApplyJump()
        {
            gravity.y = jumpPower;
            isInJump = true;
        }
        #endregion
    }
}
