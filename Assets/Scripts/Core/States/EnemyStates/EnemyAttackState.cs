using ScalePact.Core.StateMachines;
using ScalePact.Core.States;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetUpAttack(stateMachine.Attack.KnockBackForce);
        stateMachine.Animator.CrossFadeInFixedTime(EnemyHashIDs.Attack1Hash, stateMachine.BaseCrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedAnimTime(stateMachine.Animator) >= 1)
        {
            stateMachine.SwitchState(new EnemyChaseState(stateMachine));
        }

        MovementWithForces(Vector3.zero, deltaTime);
        FacePlayer();
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
