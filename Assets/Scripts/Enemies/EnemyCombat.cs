using System;
using System.Collections;
using ScalePact.Core;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Enemies
{
    public class EnemyCombat : MonoBehaviour, IAction
    {
        
        [SerializeField] float attackRange = 2f;
        [SerializeField] float attackSpeed = 1f;
        [Range(0,1)][SerializeField] float attackMoveSpeedModifier = 0.3f;
        [SerializeField] float weaponDamage = 1f;
        [SerializeField] DamageHandler weapon = null;

        Health currentTarget;
        float timeSinceLastAttack = Mathf.Infinity;
        EnemyBehaviorState defaultState = EnemyBehaviorState.NoState;

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
            while(currentTarget != null)
            {
                enemyMovement.MoveToLocation(currentTarget.transform.position, attackMoveSpeedModifier);
                yield return null;
            }
        }

        IEnumerator AttackState()
        {
            while(true)
            {
                transform.LookAt(currentTarget.transform);
                
                animator.SetTrigger(EnemyHashIDs.AttackTriggerHash);
                yield return new WaitForSeconds(attackSpeed);
            }
        }

        // public void AttackState()
        // {
        //     transform.LookAt(currentTarget.transform);
        //     if (timeSinceLastAttack > attackSpeed)
        //     {
        //         animator.SetTrigger(EnemyHashIDs.AttackTriggerHash);
        //         timeSinceLastAttack = 0;
        //     }
        // }

        private void UpdateTimers()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public bool IsInAttackRange()
        {
            return Vector3.Distance(transform.position, currentTarget.transform.position) < attackRange;
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
        }
#endif

    }
}