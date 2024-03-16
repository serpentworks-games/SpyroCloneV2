using System;

namespace ScalePact.Core.States
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void PhysicsTick(float deltaTime);
        public abstract void Exit();
        public abstract void UpdateAnimator(float deltaTime);
    }

}