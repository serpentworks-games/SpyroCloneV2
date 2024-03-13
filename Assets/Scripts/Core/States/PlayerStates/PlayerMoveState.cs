using UnityEngine;
using ScalePact.Core.StateMachines;

namespace ScalePact.Core.States
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.InputManager.JumpEvent += SwitchStateToJump;
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = new()
            {
                x = stateMachine.InputManager.MovementVector.x,
                y = 0,
                z = stateMachine.InputManager.MovementVector.y
            };
            stateMachine.CharacterController.Move(movement * stateMachine.BaseMoveSpeed * deltaTime);
            base.Tick(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            stateMachine.InputManager.JumpEvent -= SwitchStateToJump;
        }

        void SwitchStateToJump()
        {
            stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        }
    }
}