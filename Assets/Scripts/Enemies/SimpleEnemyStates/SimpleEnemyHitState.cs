using ScalePact.Core;

class SimpleEnemyHitState : SimpleEnemyBaseState
{
    float timer;
    public SimpleEnemyHitState(SimpleEnemyStateMachine stateMachine) : base(stateMachine) {
        timer = stateMachine.ImpactDuration;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        timer -= deltaTime;
        if(timer <= 0)
        {
            stateMachine.SwitchToIdleState();
        }
    }

    public override void PhysicsTick(float deltaTime)
    {
        base.PhysicsTick(deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Movement.ClearForce();
    }

}