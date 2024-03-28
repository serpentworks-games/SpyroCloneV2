using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.StateMachines.States
{
    public class EnemyImpactState : EnemyBaseState
    {
        float impactDuration;
        public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(SharedHashIDs.ImpactStateHash, stateMachine.BaseCrossFadeDuration);
            impactDuration = stateMachine.MaxImpactDuration;
        }

        public override void Tick(float deltaTime)
        {
            MovementWithForces(Vector3.zero, deltaTime);
            impactDuration -= deltaTime;
            if(impactDuration <= 0f)
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            }
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
