using ScalePact.Combat;
using ScalePact.Core.Input;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Player
{
    public class PlayerController : MonoBehaviour
    {
        readonly float animatorDampTime = 0.1f;

        Animator animator;
        InputManager inputManager;
        PlayerCombat combat;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            inputManager = GetComponent<InputManager>();
            combat = GetComponent<PlayerCombat>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            if (!combat.IsAttacking)
            {
                animator.SetFloat(PlayerHashIDs.BaseVelocityHash, inputManager.MovementVector.magnitude, animatorDampTime, Time.deltaTime);
            }
            else
            {
                animator.SetFloat(PlayerHashIDs.BaseVelocityHash, 0f, animatorDampTime, Time.deltaTime);
            }
        }
    }
}