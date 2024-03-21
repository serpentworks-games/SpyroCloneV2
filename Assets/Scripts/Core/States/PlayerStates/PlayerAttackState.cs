using ScalePact.Combat;
using ScalePact.Core.StateMachines;
using ScalePact.Core.States;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    float previousFrameTime;
    bool hasForceAlreadyBeenApplied = false;
    AttackData attackData;
    
    public PlayerAttackState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attackData = stateMachine.Attacks[attackIndex];
        stateMachine.AttackIndex = attackIndex;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(attackData.AttackName.ToString(), 0.2f);
    }

    public override void Tick(float deltaTime)
    {
        MovementWithForces(Vector3.zero, 0, deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedAnimTime(stateMachine.Animator);

        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attackData.AttackForceBlendTime)
            {
                TryToApplyForce();
            }
            if (stateMachine.InputManager.IsAttacking)
            {
                TryToCombo(normalizedTime);
            }
        }
        else
        {
            ReturnToMovementState();
        }
        
        previousFrameTime = normalizedTime;
    }

    public override void PhysicsTick(float deltaTime)
    {

    }

    public override void Exit()
    {

    }

    public override void UpdateAnimator(float deltaTime)
    {

    }

    void TryToCombo(float normalizedTime)
    {
        if (attackData.ComboStateIndex == -1) return;

        if (normalizedTime < attackData.ComboBlendTime) return;

        stateMachine.SwitchState(
            new PlayerAttackState(stateMachine, attackData.ComboStateIndex)
            );
    }

    void TryToApplyForce()
    {
        if (hasForceAlreadyBeenApplied) return;

        Vector3 forceToAdd = stateMachine.transform.forward * attackData.AttackForce;
        stateMachine.ForceReceiver.AddForce(forceToAdd);
        hasForceAlreadyBeenApplied = true;
    }
}