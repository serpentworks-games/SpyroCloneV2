using UnityEngine;

[System.Serializable]
public abstract class State
{
    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void PhysicsTick(float deltaTime);

    public abstract void Exit();

    protected float GetNormalizedAnimTime(Animator animator, string stateTagName)
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextState = animator.GetCurrentAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextState.IsTag(stateTagName))
        {
            return nextState.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentState.IsTag(stateTagName))
        {
            return currentState.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}