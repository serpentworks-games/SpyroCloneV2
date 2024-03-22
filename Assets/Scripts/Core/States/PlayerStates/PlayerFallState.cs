using System.Collections;
using System.Collections.Generic;
using ScalePact.Core.StateMachines;
using ScalePact.Core.States;
using UnityEngine;

namespace ScalePact.Core.States
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(PlayerHashIDs.JumpLandHash, 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            float normalizedTime = GetNormalizedAnimTime(stateMachine.Animator);

            if (normalizedTime < 1f)
            {

            }
            else
            {
                ReturnToMovementState();
            }

            previousFrameTime = normalizedTime;
        }

        public override void PhysicsTick(float deltaTime)
        {

        }

        public override void Exit()
        {

        }

        public override void UpdateAnimator(float deltaTime)
        {

        }
    }
}