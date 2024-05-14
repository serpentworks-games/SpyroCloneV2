using UnityEngine;

namespace ScalePact.InteractionSystem.Handlers
{
    public class SetAnimatorVariableHandler : InteractionHandler
    {
        [SerializeField] Animator animator;
        [SerializeField] VariableType variableType;
        
        [Header("Variable States")]
        [SerializeField] string variableName;
        [SerializeField] bool animBoolState;
        [SerializeField] float animFloatState;
        [SerializeField] int animIntState;

        public override void PerformInteraction()
        {
            if (!animator) return;

            switch (variableType)
            {
                case VariableType.Bool:
                    animator.SetBool(variableName, animBoolState);
                    break;
                case VariableType.Float:
                    animator.SetFloat(variableName, animFloatState);
                    break;
                case VariableType.Int:
                    animator.SetInteger(variableName, animIntState);
                    break;
                case VariableType.Trigger:
                    animator.SetTrigger(variableName);
                    break;
            }
        }

    }

    [System.Serializable]
    public enum VariableType
    {
        Bool, Float, Int, Trigger
    }
}