using ScalePact.Combat;
using ScalePact.Core;
using ScalePact.Forces;
using ScalePact.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] float baseMoveSpeed = 4f;
        [SerializeField] float maxImpactDuration = 1f;
        [SerializeField] float chaseDistance = 5f;

        Animator animator;
        NavMeshAgent agent;
        EnemyMovement movement;
        EnemyCombat combat;
        Health health;
        Ragdoll ragdoll;

        Health player;

        public BehaviourStates currentState;
        float timeSinceLastImpact = Mathf.Infinity;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            combat = GetComponent<EnemyCombat>();
            movement = GetComponent<EnemyMovement>();
            health = GetComponent<Health>();
            ragdoll = GetComponent<Ragdoll>();

        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.OnReceiveDamage += ImpactState;
            health.OnDeath += DeathState;
        }

        private void OnDisable()
        {
            health.OnReceiveDamage -= ImpactState;
            health.OnDeath -= DeathState;
        }

        private void Update()
        {
            UpdateTimers();

            if (health.IsDead) return;

            if (IsInAttackRange() && combat.CanAttack(player))
            {
                combat.Attack(player);
            }
            else
            {
                combat.CancelAction();
            }

            UpdateAnimator();
        }

        void UpdateTimers()
        {
            timeSinceLastImpact += Time.deltaTime;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat(EnemyHashIDs.SpeedHash, speed);
        }

        bool IsInAttackRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }


        #region States
        void PatrolBehaviour()
        {
            Debug.Log($"Entering Patrol State!");
        }

        void ImpactState()
        {
            Debug.Log($"Entering Impact State!");
            animator.SetTrigger(EnemyHashIDs.ImpactTriggerHash);

            if (timeSinceLastImpact > maxImpactDuration)
            {
                currentState = BehaviourStates.PATROL_STATE;
                timeSinceLastImpact = 0;
            }
        }

        void DeathState()
        {
            Debug.Log($"Entering Death State!");
            ragdoll.ToggleRagdoll(true);
            animator.SetTrigger(EnemyHashIDs.DeathTriggerHash);
            //agent.isStopped = true;
            currentState = BehaviourStates.NO_STATE;
        }
        #endregion

        #region Event Handlers
        void SwitchToImpact()
        {
            currentState = BehaviourStates.IMPACT_STATE;
            ImpactState();
        }

        void SwitchToDeath()
        {
            currentState = BehaviourStates.DEATH_STATE;
            DeathState();
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
#endif
    }
}