using System.Collections.Generic;
using Cinemachine;
using ScalePact.Core.Input;
using ScalePact.Core.States;
using UnityEngine;

namespace ScalePact.Core.StateMachines
{
    public class PlayerStateMachine : StateMachine
    {
        //Variables
        [field: Header("Base Variables")]
        [field: SerializeField] public float BaseMoveSpeed { get; private set; } = 5f;
        [field: SerializeField] public float BaseRotationSpeed { get; private set; } = 10f;

        [field: Header("Ground and Airborne Variables")]
        [field: SerializeField] public float FloorOffsetY { get; private set; }
        [field: SerializeField] public float FloorRaycastLength { get; private set; } = 1f;
        [field: SerializeField] public float RaycastWidthX { get; private set; }
        [field: SerializeField] public float RaycastWidthZ { get; private set; }

        public Transform MainCameraTransform { get; private set; }
        
        //Properties
        public InputManager InputManager { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }

        Vector3 raycastFloorPos;
        Vector3 combinedFloorRaycast;
        Vector3 gravity;

        private void Awake()
        {
            InputManager = GetComponent<InputManager>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
        }

        void Start()
        {
            MainCameraTransform = Camera.main.transform;
            SwitchState(new PlayerMoveState(this));
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public Vector3 FindFloor()
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