using ScalePact.Core.StateMachines;
using UnityEngine;
namespace ScalePact.Core.States
{
    public class PlayerDeathState : PlayerBaseState
    {
        public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(SharedHashIDs.DeathStateHash, stateMachine.BaseCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {

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