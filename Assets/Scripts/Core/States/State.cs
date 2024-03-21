using System;
using ScalePact.Core.StateMachines;
using UnityEngine;

namespace ScalePact.Core.States
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void PhysicsTick(float deltaTime);
        public abstract void Exit();
        public abstract void UpdateAnimator(float deltaTime);

        protected float GetNormalizedAnimTime(Animator animator)
        {
            AnimatorStateInfo currentAnimStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextAnimStateInfo = animator.GetNextAnimatorStateInfo(0);

            if (animator.IsInTransition(0) && nextAnimStateInfo.IsTag("Attack"))
            {
                return nextAnimStateInfo.normalizedTime;
            }

            else if (!animator.IsInTransition(0) && currentAnimStateInfo.IsTag("Attack"))
            {
                return currentAnimStateInfo.normalizedTime;
            }
            else
            {
                return 0;
            }
        }
    }

}