using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State currentState;

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