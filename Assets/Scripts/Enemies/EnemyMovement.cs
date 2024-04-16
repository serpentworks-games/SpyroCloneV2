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

        public void StartMoveAction(Vector3 destination)
        {
            actionScheduler.StartAction(this);
            MoveToLocation(destination);
        }

        public void MoveToLocation(Vector3 destination)
        {
            agent.isStopped = false;
            agent.destination = destination;
        }

        public void CancelAction()
        {
            agent.isStopped = true;
        }

        
    }
}