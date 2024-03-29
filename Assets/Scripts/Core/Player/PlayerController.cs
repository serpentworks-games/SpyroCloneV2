using ScalePact.Combat;
using ScalePact.Core.Input;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Grounded Movement")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 10f;

        [Header("Jump/Glide Movement")]
        [SerializeField] float jumpPower = 5f;
        [SerializeField] float jumpFallOff = 2f;
        [SerializeField] float lowJumpMulti = 1.5f;
        [SerializeField] float glideSpeed = 7f;
        [SerializeField] float movingGlideForce = 200f;
        [SerializeField] float stationaryGlideForce = 100f;

        [Header("Slope Tolerances")]
        [SerializeField] float slopeLimit = 45f;
        [SerializeField] float slopeInfluence = 5f;

        [Header("Finding the Ground")]
        [SerializeField] float floorOffsetY = 0.05f;
        [SerializeField] float floorRaycastLength = 1f;
        [SerializeField] float raycastWidthX = 0.5f;
        [SerializeField] float raycastWidthZ = 2f;

        [Header("Animation")]
        [SerializeField] float animatorDampTime = 0.1f;

        Camera mainCamera;

        Rigidbody rb;
        Animator animator;
        InputManager inputManager;
        TargetScanner targetScanner;
        PlayerCombat combat;

        //Base movement
        Vector3 moveDir;

        //Grounded/Jumping
        Vector3 raycastFloorPos;
        Vector3 floorMovement;
        Vector3 gravity;
        Vector3 combinedFloorRaycast;
        bool inJump;


        //Slopes
        float slopeAmount;
        Vector3 floorNormal;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            targetScanner = GetComponent<TargetScanner>();
            inputManager = GetComponent<InputManager>();
            combat = GetComponent<PlayerCombat>();

            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            inputManager.JumpEvent += OnJumpPressed;
        }

        private void OnDisable()
        {
            inputManager.JumpEvent -= OnJumpPressed;
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

            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            if (!IsGrounded() || slopeAmount >= 0.1f)
            {
                gravity += Vector3.up * Physics.gravity.y * (jumpFallOff - 1) * Time.fixedDeltaTime;
            }

            CalculateRotation();

            Vector3 velocity;

            velocity = moveDir * GetMoveSpeed();

            rb.velocity = velocity + gravity;

            floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

            //Stick to the floor if on the ground
            if (floorMovement != rb.position && IsGrounded() && rb.velocity.y <= 0)
            {
                if (inJump)
                {
                    animator.SetTrigger(PlayerHashIDs.LandTriggerHash);
                    inJump = false;
                }
                rb.MovePosition(floorMovement);
                gravity.y = 0;
            }
        }

        void UpdateAnimator()
        {
            if (!combat.IsAttacking)
            {
                animator.SetFloat(PlayerHashIDs.BaseVelocityHash, inputManager.MovementVector.magnitude, animatorDampTime, Time.deltaTime);
            }
            else
            {
                animator.SetFloat(PlayerHashIDs.BaseVelocityHash, 0f, animatorDampTime, Time.deltaTime);
            }
        }


        #region Movement
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

        void CalculateRotation()
        {
            Vector3 targetDirNormal = moveDir;
            if (moveDir == Vector3.zero)
            {
                targetDirNormal = transform.forward;
            }
            targetDirNormal.y = 0;

            Quaternion rot = Quaternion.LookRotation(targetDirNormal);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * rotationSpeed);
            transform.rotation = targetRot;
        }


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

        float GetMoveSpeed()
        {
            float currentMoveSpeed = Mathf.Clamp(moveSpeed + (slopeAmount * slopeInfluence), 0, moveSpeed + 1);
            return currentMoveSpeed;
        }
        #endregion

        void OnJumpPressed()
        {
            if (IsGrounded())
            {
                animator.SetTrigger(PlayerHashIDs.JumpTriggerHash);
            }
        }

        //Anim Events
        void ApplyJump()
        {
            gravity.y = jumpPower;
            inJump = true;
        }
    }
}