using UnityEngine;

class SimpleEnemySuspicionState : SimpleEnemyBaseState
{
    float timeSinceEnteredState;
    public SimpleEnemySuspicionState(SimpleEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        timeSinceEnteredState += deltaTime;
        
        if (timeSinceEnteredState > stateMachine.SuspicionStateTime)
        {
            stateMachine.ReturnToOriginalPosition();
            stateMachine.SwitchToIdleState();
            return;
        }
        else
        {
            stateMachine.FindTarget();
            if (stateMachine.Target != null)
            {
                stateMachine.SwitchToChaseState();
                return;
            }
        }


    }

    public override void PhysicsTick(float deltaTime)
    {
        base.PhysicsTick(deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
    }

}