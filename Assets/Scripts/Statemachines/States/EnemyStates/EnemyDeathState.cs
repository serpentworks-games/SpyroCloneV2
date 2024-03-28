using ScalePact.StateMachines;
using UnityEngine;

namespace ScalePact.StateMachines.States
{
    public class EnemyDeathState : EnemyBaseState
    {
        public EnemyDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //stateMachine.Animator.CrossFadeInFixedTime(SharedHashIDs.DeathStateHash, stateMachine.BaseCrossFadeDuration);
            stateMachine.Ragdoll.ToggleRagdoll(true);
            stateMachine.Weapon.enabled = false;
            GameObject.Destroy(stateMachine.Target);
        }

        public override void Tick(float deltaTime)
        {

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