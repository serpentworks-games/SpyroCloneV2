using ScalePact.Combat;
using ScalePact.Core.Input;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Player
{
    public class PlayerController : MonoBehaviour, IMessageReceiver
    {
        readonly float animatorDampTime = 0.1f;

        Animator animator;
        InputManager inputManager;
        PlayerCombat combat;
        PlayerMovement movement;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            inputManager = GetComponent<InputManager>();
            combat = GetComponent<PlayerCombat>();
            movement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            if (!combat.IsAttacking)
            {
                //If we are not attacking, we should be able to move
                animator.SetFloat(PlayerHashIDs.BaseVelocityHash, inputManager.MovementVector.magnitude, animatorDampTime, Time.deltaTime);
            }
            else
            {
                //If we are attacking, don't move
                animator.SetFloat(PlayerHashIDs.BaseVelocityHash, 0f, animatorDampTime, Time.deltaTime);
            }
        }

        public void OnReceiveMessage(MessageType type, object sender, object msg)
        {
            Damageable.DamageMessage data = (Damageable.DamageMessage)msg;
            switch (type)
            {
                case MessageType.DAMAGED:
                    Damaged(data);
                    break;
                case MessageType.DEAD:
                    Death(data);
                    break;
            }
        }
        void Damaged(Damageable.DamageMessage msg)
        {
            Vector3 pushForce = transform.position - msg.damageSource;
            pushForce.y = 0;
            transform.forward = -pushForce.normalized;
            movement.AddForce(pushForce, true);
        }
        void Death(Damageable.DamageMessage msg)
        {
            animator.SetTrigger(PlayerHashIDs.DeathTriggerHash);
        }
    }
}