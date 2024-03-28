using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.StateMachines.States
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(EnemyHashIDs.LocomotionHash, stateMachine.BaseCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (IsInChaseRange())
            {
                stateMachine.SwitchState(new EnemyChaseState(stateMachine));
                return;
            }

            MovementWithForces(Vector3.zero, deltaTime);

            UpdateAnimator(deltaTime);
        }

        public override void Exit()
        {

        }

        public override void UpdateAnimator(float deltaTime)
        {
            stateMachine.Animator.SetFloat(EnemyHashIDs.SpeedHash, 0f, kAnimatorDampTime, deltaTime);
        }
    }
}