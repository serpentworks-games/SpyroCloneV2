using UnityEngine;

public class SimpleEnemyBaseState : State
{
    protected SimpleEnemyStateMachine stateMachine;

    public SimpleEnemyBaseState(SimpleEnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {

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