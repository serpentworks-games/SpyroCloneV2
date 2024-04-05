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

        Health currentTarget;
        float timeSinceLastAttack = Mathf.Infinity;

        ActionScheduler actionScheduler;
        Animator animator;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateTimers();

            if (currentTarget == null) return;
            if (!GetIsInRange())
            {

            }
            else
            {
                actionScheduler.StartAction(this);
                AttackState();
            }
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
            if(timeSinceLastAttack > attackSpeed)
            {
                animator.SetTrigger(EnemyHashIDs.AttackTriggerHash);
                timeSinceLastAttack = 0;
            }
        }

        private void UpdateTimers()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, currentTarget.transform.position) < attackRange;
        }

        //Anim Events
        void EnableCollider()
        {

        }

        void DisableCollider()
        {

        }

    }
}