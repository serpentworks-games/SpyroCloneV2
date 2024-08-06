using ScalePact.Core;

class SimpleEnemyIdleState : SimpleEnemyBaseState
{
    public SimpleEnemyIdleState(SimpleEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        stateMachine.FindTarget();
        if (stateMachine.Target != null)
        {
            stateMachine.Movement.SetTarget(stateMachine.Target.transform.position);
            stateMachine.SwitchToChaseState();
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