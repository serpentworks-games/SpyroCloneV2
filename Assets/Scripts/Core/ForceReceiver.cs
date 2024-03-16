using UnityEngine;

namespace ScalePact.Core
{
    public class ForceReceiver : MonoBehaviour
    {
        [field: Header("Forces")]
        [field: SerializeField] public float Drag { get; private set; }
        //Serialized Fields
        [field: Header("Finding the Floor")]
        [field: SerializeField] public float FloorOffsetY { get; private set; }
        [field: SerializeField] public float FloorRaycastLength { get; private set; } = 1f;
        [field: SerializeField] public float RaycastWidthX { get; private set; }
        [field: SerializeField] public float RaycastWidthZ { get; private set; }

        //public properties
        public Vector3 Movement => impact + Vector3.up * verticalVelocity;

        Rigidbody rigidbody;

        Vector3 impact;
        float verticalVelocity;
        Vector3 dampingVelocity;

        Vector3 raycastFloorPos;
        Vector3 combinedFloorRaycast;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {

            if (verticalVelocity < 0f && IsGrounded())
            {
                verticalVelocity = Physics.gravity.y * Time.fixedDeltaTime;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;
            }

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, Drag);
        }

        public Vector3 GetFloorMovement()
        {
            return new Vector3(rigidbody.position.x, FindFloor().y + FloorOffsetY, rigidbody.position.z);
        }

        public void AddForce(Vector3 forceToAdd)
        {
            impact += forceToAdd;
        }

        bool IsGrounded()
        {
            if (GetFloorRaycasts(0, 0, 0.6f) != Vector3.zero)
            {
                return true;
            }
            return false;
        }

        Vector3 FindFloor()
        {
            int floorAverage = 1;

            combinedFloorRaycast = GetFloorRaycasts(0, 0, FloorRaycastLength); //Get the center first
            floorAverage +=
                GetFloorAverage(RaycastWidthX, 0) + GetFloorAverage(-RaycastWidthX, 0)
                    + GetFloorAverage(0, RaycastWidthZ) + GetFloorAverage(0, -RaycastWidthZ);

            return combinedFloorRaycast / floorAverage;
        }

        Vector3 GetFloorRaycasts(float offsetX, float offsetZ, float raycastLength)
        {
            raycastFloorPos = transform.TransformPoint(0 + offsetX, 0 + 0.5f, 0 + offsetZ); //y should be height

            Debug.DrawRay(raycastFloorPos, Vector3.down * raycastLength, Color.magenta);

            if (Physics.Raycast(raycastFloorPos, -Vector3.up, out RaycastHit hit, raycastLength))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

        int GetFloorAverage(float offsetX, float offsetZ)
        {
            if (GetFloorRaycasts(offsetX, offsetZ, FloorRaycastLength) != Vector3.zero)
            {
                combinedFloorRaycast += GetFloorRaycasts(offsetX, offsetZ, FloorRaycastLength);
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}