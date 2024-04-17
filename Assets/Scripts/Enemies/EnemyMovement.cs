using ScalePact.Core;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Enemies
{
    public class EnemyMovement : MonoBehaviour, IAction
    {
        NavMeshAgent agent;
        ActionScheduler actionScheduler;

        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
            actionScheduler = GetComponent<ActionScheduler>();
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

        
    }
}