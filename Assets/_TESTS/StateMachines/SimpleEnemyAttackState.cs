using UnityEngine;

class SimpleEnemyAttackState : SimpleEnemyBaseState
{
    Vector3 attackPos;
    float timer = 0;
    public SimpleEnemyAttackState(SimpleEnemyStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        stateMachine.Movement.SetShouldFollowAgent(false);

        attackPos = stateMachine.Target.transform.position;
        Vector3 toTarget = attackPos - stateMachine.transform.position;
        toTarget.y = 0;

        stateMachine.transform.forward = toTarget.normalized;
        stateMachine.Movement.SetForward(stateMachine.transform.forward);
    }

    public override void Tick(float deltaTime)
    {
        timer += deltaTime;

        if(timer >= stateMachine.testWaitTimeForAttack)
        {
            stateMachine.SwitchToIdleState();
        }
        base.Tick(deltaTime);
    }

    public override void PhysicsTick(float deltaTime)
    {
        base.PhysicsTick(deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Movement.SetShouldFollowAgent(true);
    }

}