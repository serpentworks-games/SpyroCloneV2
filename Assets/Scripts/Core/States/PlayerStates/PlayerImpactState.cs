using System.Collections;
using System.Collections.Generic;
using ScalePact.Core.StateMachines;
using ScalePact.Core.States;
using UnityEngine;

namespace ScalePact.Core.States
{
    public class PlayerImpactState : PlayerBaseState
    {
        float impactDuration;
        public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(SharedHashIDs.ImpactStateHash, stateMachine.BaseCrossFadeDuration);
            impactDuration = stateMachine.MaxImpactDuration;
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void PhysicsTick(float deltaTime)
        {
            MovementWithForces(Vector3.zero, 0f, deltaTime);
            impactDuration -= deltaTime;

            if (impactDuration <= 0)
            {
                ReturnToMovementState();
            }
        }

        public override void Exit()
        {

        }

        public override void UpdateAnimator(float deltaTime)
        {

        }
    }
}
