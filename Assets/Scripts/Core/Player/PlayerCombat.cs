using System;
using ScalePact.Combat;
using ScalePact.Core.Input;
using UnityEngine;

namespace ScalePact.Core.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] AttackData[] attackData;

        int currentIndex = -1;
        float previousFrameTime;
        float normalizedTime;

        InputManager inputManager;
        Animator animator;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            inputManager.LightAttackEvent += OnAttackPressed;
        }

        private void OnDisable()
        {
            inputManager.LightAttackEvent -= OnAttackPressed;
        }

        private void Update()
        {

        }

        private void OnAttackPressed()
        {

        }

        private void Attack(int index)
        {

        }

        private void AttemptComboAttack(AttackData attack)
        {

        }

        float GetNormalizedTime(Animator animator)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);

            if (animator.IsInTransition(0) && nextState.IsTag("Attack"))
            {
                return nextState.normalizedTime;
            }
            else if (!animator.IsInTransition(0) && currentState.IsTag("Attack"))
            {
                return currentState.normalizedTime;
            }
            else
            {
                return 0f;
            }
        }
    }
}