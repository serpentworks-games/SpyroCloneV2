using System.Collections.Generic;
using Cinemachine;
using ScalePact.Combat;
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
        [field: SerializeField] public float TargettedMoveSpeed { get; private set; } = 4f;

        [field: Header("Combat Data")]
        [field: SerializeField] public AttackData[] Attacks { get; private set; }


        public Transform MainCameraTransform { get; private set; }
        public int AttackIndex { get; set; }

        //Reference Properties
        public InputManager InputManager { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public TargetScanner TargetScanner { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }

        private void Awake()
        {
            InputManager = GetComponent<InputManager>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            TargetScanner = GetComponentInChildren<TargetScanner>();
            ForceReceiver = GetComponent<ForceReceiver>();
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

        public void EnableCollider()
        {
            Attacks[AttackIndex].DamageHandler.EnableCollider();
        }
        public void DisableCollider()
        {
            Attacks[AttackIndex].DamageHandler.DisableCollider();
        }
    }
}