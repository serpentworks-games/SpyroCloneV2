using UnityEngine;

public class SimpleEnemyDeathState : SimpleEnemyBaseState
{
    public SimpleEnemyDeathState(SimpleEnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime("Death", 0.1f);
        stateMachine.Movement.SetShouldFollowAgent(false);
    }

    public override void PhysicsTick(float deltaTime)
    {

    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }
}