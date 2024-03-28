using ScalePact.Combat;
using ScalePact.Core.Input;
using ScalePact.Utils;
using UnityEngine;

//TODO: Fix quick click combos breaking

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
            if (Time.time - lastClickTime > maxComboDelay)
            {
                IsAttacking = false;
                numOfClicks = 0;
            }
        }

        private void OnAttackPressed()
        {
            Debug.Log("Attack pressed!");
            lastClickTime = Time.time;
            numOfClicks++;
            IsAttacking = true;

            animator.SetTrigger(PlayerHashIDs.AttackTriggerHash);

            if (numOfClicks == 1)
            {
                SetAnimatorIndex(0);
            }

            numOfClicks = Mathf.Clamp(numOfClicks, 0, attackData.Length);

            if (numOfClicks >= 2 && GetNormalizedTime() > attackData[0].ComboBlendTime && CheckAnimStateName(attackData[0].AttackName.ToString()))
            {
                SetAnimatorIndex(1);
            }

            if (numOfClicks >= 3 && GetNormalizedTime() > attackData[1].ComboBlendTime && CheckAnimStateName(attackData[1].AttackName.ToString()))
            {
                SetAnimatorIndex(2);
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

        void SetAnimatorIndex(int index)
        {
            currentIndex = index;
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