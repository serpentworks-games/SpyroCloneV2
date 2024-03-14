using ScalePact.Core.States;
using UnityEngine;

namespace ScalePact.Core.StateMachines
{
    public class StateMachine : MonoBehaviour
    {
        State currentState;

        public virtual void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }

        public virtual void FixedUpdate()
        {
            currentState?.PhysicsTick(Time.deltaTime);
        }

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
    }
}
