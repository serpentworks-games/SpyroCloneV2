using ScalePact.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Core.StateMachines
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

        public Animator Animator { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }

        public GameObject PlayerRef { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            ForceReceiver = GetComponent<ForceReceiver>();
            CharacterController = GetComponent<CharacterController>();
            NavMeshAgent = GetComponent<NavMeshAgent>();

            PlayerRef = GameObject.FindWithTag("Player");

        }

        private void Start()
        {
            NavMeshAgent.updatePosition = false;
            NavMeshAgent.updateRotation = false;

            SwitchState(new EnemyIdleState(this));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ChaseRange);
        }

        public void EnableCollider()
        {
            Weapon.EnableCollider();
        }
        public void DisableCollider()
        {
            Weapon.DisableCollider();
        }
    }
}