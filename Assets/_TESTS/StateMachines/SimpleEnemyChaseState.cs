using UnityEngine;

class SimpleEnemyChaseState : SimpleEnemyBaseState
{
    public SimpleEnemyChaseState(SimpleEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        stateMachine.FindTarget();

        if (stateMachine.Target == null)
        {
            stateMachine.SwitchToSuspicionState();
        }
        else
        {
            stateMachine.RequestTargetPosition();

            Vector3 toTarget = stateMachine.Target.transform.position - stateMachine.transform.position;
            stateMachine.Movement.SetTarget(stateMachine.Target.transform.position);
            if (toTarget.sqrMagnitude < stateMachine.AttackRange * stateMachine.AttackRange)
            {
                stateMachine.SwitchToAttackState();
            }
            else if (stateMachine.FollowerData.assignedAttackSlot != -1)
            {
                Vector3 targetPos = stateMachine.Target.transform.position +
                    0.9f * stateMachine.AttackRange * stateMachine.FollowerData.distributor.GetWorldDirection(
                        stateMachine.FollowerData.assignedAttackSlot
                    );

                stateMachine.Movement.SetTarget(targetPos);
            }
            else
            {
                stateMachine.SwitchToSuspicionState();
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