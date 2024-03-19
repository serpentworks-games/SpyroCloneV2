using ScalePact.Core.StateMachines;
using UnityEngine;

namespace ScalePact.Core.States
{
    public abstract class EnemyBaseState : State
    {
        protected EnemyStateMachine stateMachine;

        protected const float kAnimatorDampTime = 0.1f;

        public EnemyBaseState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            Debug.Log($"Entering {this}");
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void PhysicsTick(float deltaTime)
        {

        }

        public override void Exit()
        {
            Debug.Log($"Exiting {this}");
        }

        public override void UpdateAnimator(float deltaTime)
        {

        }

        protected bool IsInChaseRange()
        {
            float distToPlayer = Vector3.Distance(stateMachine.PlayerRef.transform.position, stateMachine.transform.position);
            return distToPlayer <= stateMachine.ChaseRange;
        }

        protected void MovementWithForces(Vector3 motion, float deltaTime)
        {
            Vector3 movement = motion + stateMachine.ForceReceiver.Movement;
            stateMachine.CharacterController.Move(movement * deltaTime);
        }

        protected void MoveTowardsPlayer(float deltaTime)
        {
            stateMachine.NavMeshAgent.destination = stateMachine.PlayerRef.transform.position;
            MovementWithForces(stateMachine.NavMeshAgent.desiredVelocity.normalized * stateMachine.BaseMovementSpeed, deltaTime);
            stateMachine.NavMeshAgent.velocity = stateMachine.CharacterController.velocity;
        }

        protected void FacePlayer()
        {
            if (stateMachine.PlayerRef == null) return;

            Vector3 dirToTarget = stateMachine.PlayerRef.transform.position - stateMachine.transform.position;
            dirToTarget.y = 0;

            stateMachine.transform.rotation = Quaternion.LookRotation(dirToTarget);
        }
    }
}