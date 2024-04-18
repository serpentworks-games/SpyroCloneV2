using System;
using ScalePact.AI;
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
        [Header("Base Variables")]
        [SerializeField] float baseMoveSpeed = 4f;
        [SerializeField] float maxImpactDuration = 1f;

        [Header("Patrolling and Chasing")]
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionStateTime = 2f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] PatrolArea patrolArea = null;
        [SerializeField] float patrolPointDwellTime = 3f;
        [SerializeField] float patrolPointTolerance = 1f;

        //References
        Animator animator;
        NavMeshAgent agent;
        EnemyMovement movement;
        EnemyCombat combat;
        Health health;
        Ragdoll ragdoll;
        Health player;

        //Timers
        float timeSinceLastImpact = Mathf.Infinity;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtPatrolPoint = Mathf.Infinity;

        //Local state
        Vector3 guardPosition;
        int currentWaypointIndex = 0;

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
            guardPosition = transform.position;
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
                timeSinceLastSawPlayer = 0;
                AttackState();
            }
            else if (timeSinceLastSawPlayer < suspicionStateTime)
            {
                SuspicionState();
            }
            else if (patrolPath != null || patrolArea != null)
            {
                PatrolState();
            }
            else
            {
                GuardState();
            }

            UpdateAnimator();
        }

        void UpdateTimers()
        {
            timeSinceLastImpact += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtPatrolPoint += Time.deltaTime;
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
        void GuardState()
        {
            movement.StartMoveAction(guardPosition, baseMoveSpeed);
        }

        void PatrolState()
        {
            Vector3 nextPos;

            if (patrolArea != null)
            {
                nextPos = GetNextPointViaArea();
            }
            else
            {
                nextPos = GetNextPointViaWaypoint();
            }

            movement.StartMoveAction(nextPos, baseMoveSpeed);
        }

        void SuspicionState()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
            //Suspicion anim!
            Debug.Log("Now where did you go...");
        }

        void AttackState()
        {
            combat.Attack(player);
        }

        void ImpactState()
        {
            Debug.Log($"Entering Impact State!");
            animator.SetTrigger(EnemyHashIDs.ImpactTriggerHash);

            if (timeSinceLastImpact > maxImpactDuration)
            {
                timeSinceLastImpact = 0;
            }
        }

        void DeathState()
        {
            Debug.Log($"Entering Death State!");
            ragdoll.ToggleRagdoll(true);
            animator.SetTrigger(EnemyHashIDs.DeathTriggerHash);
            //agent.isStopped = true;
        }
        #endregion

        #region Patrol Areas
        private Vector3 GetNextPointViaArea()
        {
            if (AtRandomPoint())
            {
                GetNewRandomPoint();
            }
            return GetCurrentRandomPoint();
        }

        private bool AtRandomPoint()
        {
            float distanceToPoint = Vector3.Distance(transform.position, GetCurrentRandomPoint());
            return distanceToPoint < patrolPointTolerance;
        }

        private void GetNewRandomPoint()
        {
            patrolArea.GenerateRandomPoint();
        }

        private Vector3 GetCurrentRandomPoint()
        {
            return patrolArea.GetGeneratedPoint();
        }
        #endregion

        #region Patrol Paths
        private Vector3 GetNextPointViaWaypoint()
        {
            if (AtWaypoint())
            {
                GetNextWaypoint();
            }
            return GetCurrentWayPoint();
        }

        private bool AtWaypoint()
        {
            float distanceToPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToPoint < patrolPointTolerance;
        }

        private void GetNextWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
        #endregion

        #region Event Handlers
        void SwitchToImpact()
        {
            ImpactState();
        }

        void SwitchToDeath()
        {
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