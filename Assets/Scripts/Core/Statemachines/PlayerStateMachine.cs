using ScalePact.Combat;
using ScalePact.Core.Input;
using ScalePact.Core.States;
using ScalePact.Forces;
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
        [field: SerializeField] public float BaseCrossFadeDuration { get; private set; } = 0.1f;

        [field: Header("Jump and Glide Variables")]
        [field: SerializeField] public float JumpForce { get; private set; }

        [field: Header("Dodge Variables")]
        [field: SerializeField] public float MaxDodgeDuration { get; private set; }
        [field: SerializeField] public float DodgeCooldown { get; private set; }
        [field: SerializeField] public float MaxDodgeDistance { get; private set; }

        [field: Header("Combat Data")]
        [field: SerializeField] public float MaxImpactDuration { get; private set; } = 1f;
        [field: SerializeField] public AttackData[] Attacks { get; private set; }

        public Transform MainCameraTransform { get; private set; }
        public int AttackIndex { get; set; }
        public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;

        //Reference Properties
        public InputManager InputManager { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public TargetScanner TargetScanner { get; private set; }
        public PlayerForceReceiver ForceReceiver { get; private set; }
        public Health Health { get; private set; }

        private void Awake()
        {
            InputManager = GetComponent<InputManager>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponent<Animator>();
            TargetScanner = GetComponentInChildren<TargetScanner>();
            ForceReceiver = GetComponent<PlayerForceReceiver>();
            Health = GetComponent<Health>();
        }

        void Start()
        {
            MainCameraTransform = Camera.main.transform;
            SwitchState(new PlayerMoveState(this));
        }

        private void OnEnable()
        {
            Health.OnReceiveDamage += ApplyWhenDamaged;
            Health.OnDeath += ApplyWhenDead;
        }

        private void OnDisable()
        {
            Health.OnReceiveDamage -= ApplyWhenDamaged;
            Health.OnDeath -= ApplyWhenDead;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void SetDodgeTime(float time)
        {
            PreviousDodgeTime = time;
        }

        public void EnableCollider()
        {
            Attacks[AttackIndex].DamageHandler.EnableCollider();
        }
        public void DisableCollider()
        {
            Attacks[AttackIndex].DamageHandler.DisableCollider();
        }

        void ApplyWhenDamaged()
        {
            SwitchState(new PlayerImpactState(this));
        }

        void ApplyWhenDead()
        {
            SwitchState(new PlayerDeathState(this));
        }
    }
}