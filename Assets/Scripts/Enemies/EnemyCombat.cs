using System;
using ScalePact.Combat;
using ScalePact.Core;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Enemies
{
    public class EnemyCombat : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 2f;
        [SerializeField] float attackSpeed = 1f;
        [SerializeField] float attackMoveSpeed = 4f;
        [SerializeField] float weaponDamage = 1f;
        [SerializeField] DamageHandler weapon = null;

        Health currentTarget;
        float timeSinceLastAttack = Mathf.Infinity;

        ActionScheduler actionScheduler;
        Animator animator;
        EnemyMovement enemyMovement;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            enemyMovement = GetComponent<EnemyMovement>();
        }

        private void Update()
        {
            UpdateTimers();

            if (currentTarget == null) return;
            if (currentTarget.IsDead) return;

            if (!IsInAttackRange())
            {
                enemyMovement.StartMoveAction(currentTarget.transform.position, attackMoveSpeed);
            }
            else
            {
                actionScheduler.StartAction(this);
                AttackState();
            }
        }

        public bool CanAttack(Health target)
        {
            if (target == null) return false;
            return target != null && !target.IsDead;
        }

        public void Attack(Health target)
        {
            currentTarget = target;
        }

        public void CancelAction()
        {
            currentTarget = null;
        }

        void AttackState()
        {
            transform.LookAt(currentTarget.transform);
            if (timeSinceLastAttack > attackSpeed)
            {
                animator.SetTrigger(EnemyHashIDs.AttackTriggerHash);
                timeSinceLastAttack = 0;
            }
        }

        private void UpdateTimers()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        private bool IsInAttackRange()
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