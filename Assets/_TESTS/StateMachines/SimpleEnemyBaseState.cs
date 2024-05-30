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
        Debug.Log("Entering: " + this);
    }

    public override void PhysicsTick(float deltaTime)
    {
        Debug.Log("Ticking Physics on: " + this);
    }

    public override void Tick(float deltaTime)
    {
        Debug.Log("Ticking on: " + this);
    }

    public override void Exit()
    {
        Debug.Log("Exiting: " + this);
    }
}