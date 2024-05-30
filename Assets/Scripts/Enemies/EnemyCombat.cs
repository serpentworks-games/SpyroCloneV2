using System;
using System.Collections;
using ScalePact.Combat;
using ScalePact.Core;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Enemies
{
    public class EnemyCombat : MonoBehaviour, IAction
    {
        [SerializeField] EnemyTargetScanner attackRangeScanner;
        [SerializeField] float attackRange = 2f;
        [SerializeField] float attackSpeed = 1f;
        [Range(0, 1)][SerializeField] float attackMoveSpeedModifier = 0.3f;
        [SerializeField] float weaponDamage = 1f;
        [SerializeField] DamageHandler weapon = null;
        [SerializeField] TargetDistributor.TargetFollower followerData;

        Health currentTarget;
        Health player;
        float timeSinceLastAttack = Mathf.Infinity;
        EnemyBehaviorState defaultState = EnemyBehaviorState.AttackIdleState;

        Animator animator;
        EnemyMovement enemyMovement;
        ActionScheduler actionScheduler;

        Coroutine combatCo;
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

        public delegate void StateChangeEvent(EnemyBehaviorState oldState, EnemyBehaviorState newState);
        public StateChangeEvent OnStateChange;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyMovement = GetComponent<EnemyMovement>();
            actionScheduler = GetComponent<ActionScheduler>();

            player = GameObject.FindWithTag("Player").GetComponent<Health>();

            OnStateChange += HandleStateChange;
        }

        private void Update()
        {
            UpdateTimers();

            if (currentTarget == null) return;
            if (currentTarget.IsDead) return;

            if (!IsInAttackRange())
            {
                State = EnemyBehaviorState.AttackIdleState;
            }
            else
            {
                enemyMovement.CancelAction();
                State = EnemyBehaviorState.AttackState;
            }
        }

        void HandleStateChange(EnemyBehaviorState oldState, EnemyBehaviorState newState)
        {
            if (oldState != newState)
            {
                if (combatCo != null)
                {
                    StopCoroutine(combatCo);
                }

                switch (newState)
                {
                    case EnemyBehaviorState.AttackState:
                        combatCo = StartCoroutine(AttackState());
                        break;
                    case EnemyBehaviorState.AttackIdleState:
                        combatCo = StartCoroutine(CloseDistance());
                        break;
                }
            }
        }

        public bool CanAttack(Health target)
        {
            if (target == null) return false;
            return target != null && !target.IsDead;
        }

        public void Attack(Health target)
        {
            actionScheduler.StartAction(this);
            currentTarget = target;
        }

        public void CancelAction()
        {
            currentTarget = null;
        }

        IEnumerator CloseDistance()
        {
            while (currentTarget != null)
            {
                enemyMovement.MoveToLocation(currentTarget.transform.position, attackMoveSpeedModifier);
                yield return null;
            }
        }

        IEnumerator AttackState()
        {
            while (true)
            {
                transform.LookAt(currentTarget.transform);

                animator.SetTrigger(EnemyHashIDs.AttackTriggerHash);
                yield return new WaitForSeconds(attackSpeed);
            }
        }

        private void UpdateTimers()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public bool IsInAttackRange()
        {
            return attackRangeScanner.Detect(transform, player) != null;    
        }

        void GetTarget()
        {
            Health target = attackRangeScanner.Detect(transform, currentTarget == null);
            if(target != null) currentTarget = target;
        }

        //Anim Events
        void EnableCollider()
        {
            weapon.EnableCollider();
        }

        void DisableCollider()
        {
            weapon.DisableCollider();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            attackRangeScanner.EditorGizmo(transform);
        }
#endif

    }
}