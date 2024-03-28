using ScalePact.Combat;
using ScalePact.Combat.Forces;
using ScalePact.StateMachines.States;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.StateMachines
{
    public class EnemyStateMachine : StateMachine
    {
        [field: Header("Player Detection")]
        [field: SerializeField] public float ChaseRange { get; private set; } = 5f;

        [field: Header("Combat Variables")]
        [field: SerializeField] public DamageHandler Weapon { get; private set; }
        [field: SerializeField] public AttackData Attack { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; } = 2f;
        [field: SerializeField] public float MaxImpactDuration { get; private set; } = 1f;

        [field: Header("Other Variables")]
        [field: SerializeField] public float BaseMovementSpeed { get; private set; } = 6f;
        [field: SerializeField] public float BaseCrossFadeDuration { get; private set; } = 0.1f;

        public float PreviousAttackTime { get; private set; } = Mathf.NegativeInfinity;

        public Animator Animator { get; private set; }
        public EnemyForceReceiver ForceReceiver { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Health Health { get; private set; }
        public Target Target { get; private set; }
        public Ragdoll Ragdoll { get; private set; }

        public Health PlayerRef { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            ForceReceiver = GetComponent<EnemyForceReceiver>();
            CharacterController = GetComponent<CharacterController>();
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Health = GetComponent<Health>();
            Target = GetComponent<Target>();
            Ragdoll = GetComponent<Ragdoll>();

            PlayerRef = GameObject.FindWithTag("Player").GetComponent<Health>();

        }

        private void Start()
        {
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;

            SwitchState(new EnemyIdleState(this));
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

        public void SetAttackTime(float time)
        {
            PreviousAttackTime = time;
        }

        public void EnableCollider()
        {
            Weapon.EnableCollider();
        }
        public void DisableCollider()
        {
            Weapon.DisableCollider();
        }

        void ApplyWhenDamaged()
        {
            SwitchState(new EnemyImpactState(this));
        }

        void ApplyWhenDead()
        {
            SwitchState(new EnemyDeathState(this));
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ChaseRange);
        }
#endif
    }
}