using UnityEngine;
using ScalePact.Core.StateMachines;
using UnityEngine.EventSystems;
using UnityEditor.Callbacks;

namespace ScalePact.Core.States
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //stateMachine.InputManager.JumpEvent += SwitchStateToJump;
            stateMachine.InputManager.ToggleTargetEvent += SwitchToTargetting;
            stateMachine.Animator.CrossFadeInFixedTime(PlayerHashIDs.FreeLookMoveHash, stateMachine.BaseCrossFadeDuration);
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            UpdateAnimator(deltaTime);
            base.Tick(deltaTime);
        }

        public override void PhysicsTick(float deltaTime)
        {
            if (stateMachine.InputManager.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
                return;
            }
            
            Vector3 motion = CalculateFreeLookMovementVector();
            MovementWithForces(motion, stateMachine.BaseMoveSpeed, deltaTime);
            ApplyFreeLookRotation(motion, deltaTime);

            base.PhysicsTick(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            //stateMachine.InputManager.JumpEvent -= SwitchStateToJump;
            stateMachine.InputManager.ToggleTargetEvent -= SwitchToTargetting;
        }

        public override void UpdateAnimator(float deltaTime)
        {
            if (stateMachine.InputManager.MovementVector == Vector2.zero)
            {
                stateMachine.Animator.SetFloat(PlayerHashIDs.BaseVelocityHash, 0, kAnimatorDampTime, deltaTime);
                return;
            }
            stateMachine.Animator.SetFloat(PlayerHashIDs.BaseVelocityHash, 1, kAnimatorDampTime, deltaTime);

            base.UpdateAnimator(deltaTime);
        }

        void SwitchStateToJump()
        {
            stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        }

        void SwitchToTargetting()
        {
            if (!stateMachine.TargetScanner.SelectTarget()) return;
            stateMachine.SwitchState(new PlayerTargetState(stateMachine));
        }

    }
}