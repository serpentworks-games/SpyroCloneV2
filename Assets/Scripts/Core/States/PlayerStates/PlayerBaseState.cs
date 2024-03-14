using ScalePact.Core.StateMachines;
using UnityEngine;

namespace ScalePact.Core.States
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;

        protected const float kAnimatorDampTime = 0.1f;

        public PlayerBaseState(PlayerStateMachine stateMachine)
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

    }
}