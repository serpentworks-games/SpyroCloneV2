using System;
using System.Collections;
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
        [SerializeField] float maxImpactDuration = 1f;
        [SerializeField] EnemyBehaviorState defaultState = EnemyBehaviorState.NoState;

        [Header("Speed Modifiers")]
        [Range(0, 1)][SerializeField] float patrollingSpeedModifier = 0.2f;

        [Header("Chasing Varriables")]
        [SerializeField] EnemyTargetScanner chaseRangeScanner;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionStateTime = 10f;

        [Header("Patrol Variables")]
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] PatrolArea patrolArea = null;
        [SerializeField] float patrolPointDwellTime = 3f;
        [SerializeField] float patrolPointTolerance = 1f;

        //References
        Animator animator;
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
        Coroutine movementCo;
        EnemyBehaviorState currentState;
        public EnemyBehaviorState State
        {
            get => currentState;
            set
            {
                OnStateChange?.Invoke(currentState, value);
                currentState = value;
            }
        }

        //Events
        public delegate void StateChangeEvent(EnemyBehaviorState oldState, EnemyBehaviorState newState);
        public StateChangeEvent OnStateChange;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            combat = GetComponent<EnemyCombat>();
            movement = GetComponent<EnemyMovement>();
            health = GetComponent<Health>();
            ragdoll = GetComponent<Ragdoll>();

            OnStateChange += HandleStateChange;
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Health>();
            guardPosition = transform.position;
        }

        private void OnEnable()
        {
            health.OnReceiveDamage += SwitchToImpact;
            health.OnDeath += SwitchToDeath;
        }

        private void OnDisable()
        {
            health.OnReceiveDamage -= SwitchToImpact;
            health.OnDeath -= SwitchToDeath;
        }

        private void Update()
        {
            if (health.IsDead) return;
            if(!combat.CanAttack(player)) return;

            if (IsInChaseRange())
            {
                //ChaseState();
                State = EnemyBehaviorState.ChaseState;
            }
            else if (timeSinceLastSawPlayer < suspicionStateTime)
            {
                //SuspicionState();
                State = EnemyBehaviorState.SuspicionState;
            }
            else
            {
                //PatrolState();
                State = EnemyBehaviorState.PatrolState;
            }

            UpdateTimers();
        }

        void UpdateTimers()
        {
            timeSinceLastImpact += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtPatrolPoint += Time.deltaTime;
        }

        bool IsInChaseRange()
        {
            return chaseRangeScanner.Detect(transform, player == null);
        }

        #region States

        void HandleStateChange(EnemyBehaviorState oldState, EnemyBehaviorState newState)
        {
            if (oldState != newState)
            {
                if (movementCo != null)
                {
                    StopCoroutine(movementCo);
                }

                switch (newState)
                {
                    case EnemyBehaviorState.NoState:
                        break;
                    case EnemyBehaviorState.PatrolState:
                        movementCo = StartCoroutine(PatrolState());
                        break;
                    case EnemyBehaviorState.SuspicionState:
                        movementCo = StartCoroutine(SuspicionState());
                        break;
                    case EnemyBehaviorState.ChaseState:
                        movementCo = StartCoroutine(ChaseState());
                        break;
                    case EnemyBehaviorState.ImpactState:
                        ImpactState();
                        break;
                    case EnemyBehaviorState.DeathState:
                        DeathState();
                        break;
                }
            }
        }

        IEnumerator PatrolState()
        {
            Vector3 nextPos = guardPosition;

            while (true)
            {
                if (patrolArea != null)
                {
                    nextPos = GetNextPointViaArea();
                }

                else if (patrolPath != null)
                {
                    nextPos = GetNextPointViaWaypoint();
                }

                movement.StartMoveAction(nextPos, patrollingSpeedModifier);

                yield return new WaitForSeconds(patrolPointDwellTime);
            }
        }

        IEnumerator SuspicionState()
        {
            while (true)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();

                yield return new WaitForSeconds(suspicionStateTime);
                
                State = EnemyBehaviorState.PatrolState;
            }           
        }

        IEnumerator ChaseState()
        {
            combat.Attack(player);

            while (true)
            {
                timeSinceLastSawPlayer = 0;
                yield return null;
            }
        }

        // void PatrolState()
        // {
        //     Vector3 nextPos = guardPosition;

        //     if (patrolArea != null)
        //     {
        //         nextPos = GetNextPointViaArea();
        //     }

        //     if (patrolPath != null)
        //     {
        //         nextPos = GetNextPointViaWaypoint();
        //     }

        //     if (timeSinceArrivedAtPatrolPoint > patrolPointDwellTime)
        //     {
        //         movement.StartMoveAction(nextPos, patrollingSpeedModifier);
        //     }
        // }

        // void SuspicionState()
        // {
        //     GetComponent<ActionScheduler>().CancelCurrentAction();
        //     //Suspicion anim!
        // }

        // void ChaseState()
        // {
        //     timeSinceLastSawPlayer = 0;
        //     combat.Attack(player);
        // }

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
        }
        #endregion

        #region Patrol Areas
        private Vector3 GetNextPointViaArea()
        {
            if (AtRandomPoint())
            {
                timeSinceArrivedAtPatrolPoint = 0;
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
                timeSinceArrivedAtPatrolPoint = 0;
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
            State = EnemyBehaviorState.ImpactState;
        }

        void SwitchToDeath()
        {
            State = EnemyBehaviorState.DeathState;
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            chaseRangeScanner.EditorGizmo(transform);
        }
#endif
    }
}