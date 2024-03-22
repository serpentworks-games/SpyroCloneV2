using System.Collections;
using System.Collections.Generic;
using ScalePact.Core.StateMachines;
using ScalePact.Core.States;
using UnityEngine;

namespace ScalePact.Core.States
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(PlayerHashIDs.JumpStartHash, stateMachine.BaseCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            float normalizedTime = GetNormalizedAnimTime(stateMachine.Animator);

            if (normalizedTime < 1f)
            {

            }
            else
            {
                stateMachine.SwitchState(new PlayerFallState(stateMachine));
            }

            previousFrameTime = normalizedTime;
        }

        public override void PhysicsTick(float deltaTime)
        {
            MovementWithForces(Vector3.zero, 0, deltaTime);
        }

        public override void Exit()
        {

        }

        public override void UpdateAnimator(float deltaTime)
        {

        }
    }
}
