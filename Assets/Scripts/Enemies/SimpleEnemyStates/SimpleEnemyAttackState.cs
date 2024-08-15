using UnityEngine;

class SimpleEnemyAttackState : SimpleEnemyBaseState
{
    Vector3 attackPos;

    public SimpleEnemyAttackState(SimpleEnemyStateMachine stateMachine) : base(stateMachine) { }

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
        if (GetNormalizedAnimTime(stateMachine.Animator, "Attack") >= 1)
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

        stateMachine.Movement.SetShouldFollowAgent(true);
        base.Exit();
    }

}