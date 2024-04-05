using System;
using ScalePact.Core;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] float baseMoveSpeed = 4f;

        Animator animator;
        NavMeshAgent agent;

        BehaviourStates currentState;

        private void Awake() {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update() {
            //update timers

            switch (currentState)
            {
                case BehaviourStates.DEATH_STATE:
                    //be dead
                case BehaviourStates.IMPACT_STATE:
                    //trigger impact
                case BehaviourStates.ATTACK_STATE:
                    //AttackBehaviour();
                case BehaviourStates.CHASE_STATE:
                    //ChaseBehaviour();
                case BehaviourStates.SUSPICION_STATE:
                    //SuspicionBehaviour();
                case BehaviourStates.PATROL_STATE:
                    //PatrolBehaviour();
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

        private void UpdateAnimator()
        {
            
        }
    }
}