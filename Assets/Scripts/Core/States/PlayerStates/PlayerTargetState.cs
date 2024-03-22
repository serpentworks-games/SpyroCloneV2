using System;
using ScalePact.Core.StateMachines;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace ScalePact.Core.States
{
    public class PlayerTargetState : PlayerBaseState
    {
      
        public PlayerTargetState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.InputManager.ToggleTargetEvent += SwitchToFreeLook;
            stateMachine.InputManager.DodgeEvent += ApplyDodge;
            stateMachine.InputManager.JumpEvent += SwitchStateToJump;
            stateMachine.Animator.CrossFadeInFixedTime(PlayerHashIDs.TargettingMoveHash, stateMachine.BaseCrossFadeDuration);
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.InputManager.IsAttacking)
            {
                stateMachine.SwitchState(new PlayerAttackState(stateMachine, 0));
                return;
            }
            if (stateMachine.TargetScanner.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerMoveState(stateMachine));
                return;
            }

            UpdateAnimator(deltaTime);
            base.Tick(deltaTime);
        }

        public override void PhysicsTick(float deltaTime)
        {
            MovementWithForces(CalculateTargettedMovement(deltaTime), stateMachine.TargettedMoveSpeed, deltaTime);
            FaceTarget();
            base.PhysicsTick(deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputManager.ToggleTargetEvent -= SwitchToFreeLook;
            stateMachine.InputManager.DodgeEvent -= ApplyDodge;
            stateMachine.InputManager.JumpEvent -= SwitchStateToJump;
            base.Exit();
        }

        public override void UpdateAnimator(float deltaTime)
        {

            if (stateMachine.InputManager.MovementVector.y == 0)
            {
                stateMachine.Animator.SetFloat(PlayerHashIDs.TargetForwardVelocityHash, 0, kAnimatorDampTime, deltaTime);
            }
            else
            {
                float value = stateMachine.InputManager.MovementVector.y > 0 ? 1f : -1f;
                stateMachine.Animator.SetFloat(PlayerHashIDs.TargetForwardVelocityHash, value, kAnimatorDampTime, deltaTime);
            }

            if (stateMachine.InputManager.MovementVector.x == 0)
            {
                stateMachine.Animator.SetFloat(PlayerHashIDs.TargetRightVelocityHash, 0, kAnimatorDampTime, deltaTime);
            }
            else
            {
                float value = stateMachine.InputManager.MovementVector.x > 0 ? 1f : -1f;
                stateMachine.Animator.SetFloat(PlayerHashIDs.TargetRightVelocityHash, value, kAnimatorDampTime, deltaTime);
            }

            base.UpdateAnimator(deltaTime);
        }

        private void SwitchToFreeLook()
        {
            stateMachine.TargetScanner.ClearCurrentTarget();
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }

        void SwitchStateToJump()
        {
            stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        }

        void ApplyDodge()
        {
            if(Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCooldown) return;

            stateMachine.SetDodgeTime(Time.time);
            dodgeInput = stateMachine.InputManager.MovementVector;
            remainingDodgeTime = stateMachine.MaxDodgeDuration;
        }
    }
}