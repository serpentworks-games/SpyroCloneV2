using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.StateMachines.States
{
    public class EnemyChaseState : EnemyBaseState
    {
        public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(EnemyHashIDs.LocomotionHash, stateMachine.BaseCrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if (!IsInChaseRange())
            {
                stateMachine.SwitchState(new EnemyIdleState(stateMachine));
                return;
            }
            else if(IsInAttackRange())
            {
                stateMachine.SwitchState(new EnemyAttackState(stateMachine));
                return;
            }
            
            MoveTowardsPlayer(deltaTime);

            FacePlayer();

            UpdateAnimator(deltaTime);
        }

        public override void Exit()
        {
            if (stateMachine.NavMeshAgent.enabled == true)
            {
                stateMachine.NavMeshAgent.ResetPath();
                stateMachine.NavMeshAgent.velocity = Vector3.zero;
            }
        }

        public override void UpdateAnimator(float deltaTime)
        {
            stateMachine.Animator.SetFloat(EnemyHashIDs.SpeedHash, 1f, kAnimatorDampTime, deltaTime);
        }

        bool IsInAttackRange()
        {
            if (stateMachine.PlayerRef.IsDead) return false;
            
            return Vector3.Distance(stateMachine.PlayerRef.transform.position, stateMachine.transform.position) <= stateMachine.AttackRange;
        }
    }
}