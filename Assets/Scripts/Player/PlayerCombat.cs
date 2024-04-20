using System.Collections;
using ScalePact.Combat;
using ScalePact.Core;
using ScalePact.Core.Input;
using ScalePact.Forces;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] AttackData[] attackData;

        public bool IsAttacking { get; private set; } = false;

        float comboMinDelay = 0.1f;
        int comboMaxIndex = 2;
        int comboIndex;
        Coroutine comboAttackResetCoroutine;

        InputManager inputManager;
        Animator animator;
        TargetScanner targetScanner;
        PlayerForceReceiver forceReceiver;
        Health health;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            animator = GetComponent<Animator>();
            targetScanner = GetComponent<TargetScanner>();
            forceReceiver = GetComponent<PlayerForceReceiver>();
            health = GetComponent<Health>();

            comboIndex = -1;
            comboAttackResetCoroutine = null;
        }

        private void OnEnable()
        {
            inputManager.LightAttackEvent += OnLightAttackPressed;
        }

        private void OnDisable()
        {
            inputManager.LightAttackEvent -= OnLightAttackPressed;
        }

        private void Update()
        {

        }

        private void OnLightAttackPressed()
        {
            if (health.IsDead) return;

            IsAttacking = true;

            Collider closestTarget = targetScanner.GetClosestTargetNoTargetting();
            if (closestTarget != null)
            {
                forceReceiver.FaceTarget(closestTarget.transform);
            }

            if(comboIndex == comboMaxIndex) return;

            float normalizedTime = GetNormalizedTime();

            if(comboIndex == -1 || (normalizedTime > 0.1f && normalizedTime <= 0.8f))
            {
                if(comboAttackResetCoroutine != null)
                {
                    StopCoroutine(comboAttackResetCoroutine);
                }

                comboIndex++;
                SetIndexKnockBackAndForce(comboIndex);
                comboAttackResetCoroutine = StartCoroutine(ResetAttackCombo());
            }
        }

        IEnumerator ResetAttackCombo()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(animator.GetAnimatorTransitionInfo(0).duration);
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => GetNormalizedTime() >= 0.95f);
            comboIndex = -1;
            animator.SetInteger(PlayerHashIDs.AttackIndexHash, comboIndex);
            IsAttacking = false;
        }

        float GetNormalizedTime()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        void SetIndexKnockBackAndForce(int index)
        {
            attackData[index].DamageHandler.SetUpAttack(attackData[index].KnockBackForce);
            forceReceiver.AddForce(transform.forward * attackData[index].AttackForce);
            animator.SetInteger(PlayerHashIDs.AttackIndexHash, index);
        }

        //Anim Events
        void EnableCollider()
        {
            if(comboIndex == -1) return;
            attackData[comboIndex].DamageHandler.EnableCollider();
        }

        void DisableCollider()
        {
            foreach (AttackData attack in attackData)
            {
                attack.DamageHandler.DisableCollider();
            }
        }
    }
}