using System;
using ScalePact.Combat;
using ScalePact.Core;
using ScalePact.Forces;
using ScalePact.StateMachines.States;
using ScalePact.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] float baseMoveSpeed = 4f;
        [SerializeField] float maxImpactDuration = 1f;

        Animator animator;
        NavMeshAgent agent;
        EnemyMovement movement;
        EnemyCombat combat;
        Health health;
        Ragdoll ragdoll;

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
            UpdateTimers();

            switch (currentState)
            {
                case BehaviourStates.DEATH_STATE:
                    DeathState();
                    break;
                case BehaviourStates.IMPACT_STATE:
                    ImpactState();
                    break;
                case BehaviourStates.ATTACK_STATE:
                //AttackBehaviour();
                case BehaviourStates.CHASE_STATE:
                //ChaseBehaviour();
                case BehaviourStates.SUSPICION_STATE:
                //SuspicionBehaviour();
                case BehaviourStates.PATROL_STATE:
                    PatrolBehaviour();
                    return;
            }
            // if dead, do nothing
            // if impacted, impact state
            // if attacking, attack state
            // if suspicious, suspicion state
            // if chasing, chase state
            // if patrolling, patrol state
            // if nothing, do nothing

            UpdateAnimator();
        }

        void UpdateTimers()
        {
            timeSinceLastImpact += Time.deltaTime;
        }

        private void UpdateAnimator()
        {

        }

        void SwitchToImpact()
        {
            currentState = BehaviourStates.IMPACT_STATE;
        }

        void SwitchToDeath()
        {
            currentState = BehaviourStates.DEATH_STATE;
        }

        void PatrolBehaviour()
        {

        }

        void ImpactState()
        {
            animator.SetTrigger(EnemyHashIDs.ImpactTriggerHash);

            if (timeSinceLastImpact > maxImpactDuration)
            {
                currentState = BehaviourStates.PATROL_STATE;
                timeSinceLastImpact = 0;
            }
        }

        void DeathState()
        {
            //ragdoll.ToggleRagdoll(true);
            animator.SetTrigger(EnemyHashIDs.DeathTriggerHash);
            agent.isStopped = true;
            currentState = BehaviourStates.NO_STATE;
        }
    }
}