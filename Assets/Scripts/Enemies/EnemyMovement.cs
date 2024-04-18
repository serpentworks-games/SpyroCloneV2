using ScalePact.Core;
using ScalePact.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Enemies
{
    public class EnemyMovement : MonoBehaviour, IAction
    {
        Health health;
        Animator animator;
        NavMeshAgent agent;
        ActionScheduler actionScheduler;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            agent.enabled = !health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float moveSpeed)
        {
            actionScheduler.StartAction(this);
            MoveToLocation(destination, moveSpeed);
        }

        public void MoveToLocation(Vector3 destination, float moveSpeed)
        {
            agent.isStopped = false;
            agent.destination = destination;
            agent.speed = moveSpeed;
        }

        public void CancelAction()
        {
            agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat(EnemyHashIDs.SpeedHash, speed);
        }
    }
}