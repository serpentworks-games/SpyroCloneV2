using UnityEngine;

public class SimpleEnemyDeathState : SimpleEnemyBaseState
{
    public SimpleEnemyDeathState(SimpleEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime("Death", 0.1f);
        stateMachine.Movement.SetDeathRBSettings();

    }

    public override void PhysicsTick(float deltaTime)
    {
        if (GetNormalizedAnimTime(stateMachine.Animator, "Death") < 0.1)
        {
            stateMachine.Movement.ClearForce();
            stateMachine.Movement.SetShouldFollowAgent(false);
            stateMachine.Animator.enabled = false;
            stateMachine.DestroyAfterTime();
            return;
        }
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }
}