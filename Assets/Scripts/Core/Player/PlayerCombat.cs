using ScalePact.Combat;
using ScalePact.Core.Input;
using ScalePact.Forces;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Core.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] AttackData[] attackData;

        public bool IsAttacking { get; private set; } = false;

        int currentIndex = 0;
        int numOfClicks = 0;
        float lastClickTime = 0;
        float maxComboDelay = 1f;

        InputManager inputManager;
        Animator animator;
        TargetScanner targetScanner;
        PlayerForceReceiver forceReceiver;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            animator = GetComponent<Animator>();
            targetScanner = GetComponent<TargetScanner>();
            forceReceiver = GetComponent<PlayerForceReceiver>();
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
            if (Time.time - lastClickTime > maxComboDelay)
            {
                IsAttacking = false;
                numOfClicks = 0;
            }
        }

        private void OnAttackPressed()
        {
            lastClickTime = Time.time;
            numOfClicks++;
            IsAttacking = true;

            animator.SetTrigger(PlayerHashIDs.AttackTriggerHash);

            if (numOfClicks == 1)
            {
                SetIndexKnockBackAndForce(0);
            }

            numOfClicks = Mathf.Clamp(numOfClicks, 0, attackData.Length);

            if (numOfClicks >= 2 && GetNormalizedTime() > attackData[0].ComboBlendTime && CheckAnimStateName(attackData[0].AttackName.ToString()))
            {
                SetIndexKnockBackAndForce(1);
            }

            if (numOfClicks >= 3 && GetNormalizedTime() > attackData[1].ComboBlendTime && CheckAnimStateName(attackData[1].AttackName.ToString()))
            {
                SetIndexKnockBackAndForce(2);
            }

        }

        float GetNormalizedTime()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        bool CheckAnimStateName(string name)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
        }

        void SetIndexKnockBackAndForce(int index)
        {
            currentIndex = index;
            attackData[index].DamageHandler.SetUpAttack(attackData[index].KnockBackForce);
            animator.SetInteger(PlayerHashIDs.AttackIndexHash, index);
        }

        //Anim Events
        void EnableCollider()
        {
            attackData[currentIndex].DamageHandler.EnableCollider();
        }

        void DisableCollider()
        {
            attackData[currentIndex].DamageHandler.DisableCollider();
        }
    }
}