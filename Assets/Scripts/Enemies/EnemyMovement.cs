using ScalePact.Core;
using ScalePact.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Enemies
{
    public class EnemyMovement : MonoBehaviour, IAction
    {
        [SerializeField] float maxMoveSpeed = 4f;

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

        public void StartMoveAction(Vector3 destination, float speedModifier)
        {
            actionScheduler.StartAction(this);
            MoveToLocation(destination, speedModifier);
        }

        public void MoveToLocation(Vector3 destination, float speedModifier)
        {
            agent.isStopped = false;
            agent.destination = destination;
            agent.speed = maxMoveSpeed * speedModifier;
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