using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
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
        [Header("Targeting Variables")]
        [SerializeField] float targettingRadius;
        [SerializeField] LayerMask targettingLayer;

        [Header("Attacks and Abilities")]
        [SerializeField] MeleeWeapon[] meleeWeaponCombo;

        public bool IsAttacking { get; private set; } = false;
        public Collider ActiveTarget { get; private set; }
        public List<Collider> TargetColliders { get; private set; } = new();

        int targetIndex;
        bool isLockedOn;
        float comboMinDelay = 0.1f;
        int comboMaxIndex = 2;
        int comboIndex;

        Collider[] overlappedColliders;

        GameObject targetFollow;
        CinemachineTargetGroup targetGroup;

        Coroutine comboAttackResetCoroutine;
        InputManager inputManager;
        Animator animator;
        PlayerForceReceiver forceReceiver;
        Damageable damageable;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            animator = GetComponent<Animator>();
            forceReceiver = GetComponent<PlayerForceReceiver>();
            damageable = GetComponentInChildren<Damageable>();
            targetGroup = FindObjectOfType<CinemachineStateDrivenCamera>().GetComponentInChildren<CinemachineTargetGroup>();

            comboIndex = -1;
            comboAttackResetCoroutine = null;
        }

        private void OnEnable()
        {
            inputManager.LightAttackEvent += OnLightAttackPressed;
            inputManager.ToggleTargetEvent += OnLockOnTarget;
            inputManager.SwitchTargetEvent += OnSwitchTarget;
        }

        private void OnDisable()
        {
            inputManager.LightAttackEvent -= OnLightAttackPressed;
            inputManager.ToggleTargetEvent -= OnLockOnTarget;
            inputManager.SwitchTargetEvent -= OnSwitchTarget;
        }

        private void Start()
        {
            targetFollow = new()
            {
                name = "TargetFollow"
            };

            targetGroup.AddMember(targetFollow.transform, 1, 1);
        }

        private void Update()
        {
            GetPotentialTargets();

            if (isLockedOn)
            {
                targetFollow.transform.position = Vector3.Lerp(targetFollow.transform.position, ActiveTarget.bounds.center, Time.deltaTime * 4f); //HISS magic number
            }
        }

        #region Attacks
        private void OnLightAttackPressed()
        {
            if (damageable.IsDead) return;

            IsAttacking = true;

            Collider closestTarget = GetClosestTargetNoTargetting();
            if (closestTarget != null)
            {
                forceReceiver.FaceTarget(closestTarget.transform);
            }

            if (comboIndex == comboMaxIndex) return;

            float normalizedTime = GetNormalizedTime();

            if (comboIndex == -1 || (normalizedTime > 0.1f && normalizedTime <= 0.8f))
            {
                if (comboAttackResetCoroutine != null)
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
            //attackData[index].DamageHandler.SetUpAttack(attackData[index].KnockBackForce);
            //forceReceiver.AddForce(transform.forward * attackData[index].AttackForce);
            animator.SetInteger(PlayerHashIDs.AttackIndexHash, index);
        }
        #endregion

        #region Abilities
        //ABILITES HERE?
        #endregion

        #region Target Scanner
        public Collider GetClosestTargetNoTargetting()
        {
            return GetClosestTarget();
        }

        private void GetPotentialTargets()
        {
            overlappedColliders = Physics.OverlapSphere(
                transform.position, targettingRadius, 
                targettingLayer, QueryTriggerInteraction.Ignore);

            TargetColliders.Clear();

            for (int i = 0; i < overlappedColliders.Length; i++)
            {
                var overlapped = 
                    overlappedColliders[i].GetComponentInChildren<Damageable>();
                if (overlapped != null)
                {
                    TargetColliders.Add(overlappedColliders[i]);
                }
            }
        }

        void SortTargetList()
        {
            TargetColliders = TargetColliders.OrderBy(
                x => Camera.main.WorldToScreenPoint(x.transform.position).x).ToList();
        }

        Collider GetClosestTarget()
        {
            if (TargetColliders.Count == 0) return null;

            Collider bestTarget = null;
            TargetColliders = TargetColliders.OrderBy(
                x => Vector3.Distance(transform.position, x.transform.position)).ToList();
            if (TargetColliders[0] != null)
            {
                bestTarget = TargetColliders[0];
            }
            return bestTarget;
        }

        bool CheckForTargets()
        {
            if (TargetColliders.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Event Callbacks
        void OnLockOnTarget()
        {
            if (!CheckForTargets())
            {
                isLockedOn = false;
                return;
            }

            isLockedOn = !isLockedOn;
            if (isLockedOn)
            {
                ActiveTarget = GetClosestTarget();
            }
        }

        void OnSwitchTarget()
        {
            if (isLockedOn)
            {
                SortTargetList();
                if (targetIndex < (TargetColliders.Count - 1))
                {
                    targetIndex = targetIndex + 1;
                }
                else
                {
                    targetIndex = TargetColliders.Count - 1;
                }
                ActiveTarget = TargetColliders[targetIndex];
            }
        }
        #endregion

        #region Anim Events
        void EnableCollider()
        {
            // if (comboIndex == -1) return;
            // attackData[comboIndex].DamageHandler.EnableCollider();
        }

        void DisableCollider()
        {
            // foreach (AttackData attack in attackData)
            // {
            //     attack.DamageHandler.DisableCollider();
            // }
        }

        void Hit()
        {
            if (comboIndex == -1) return;
            meleeWeaponCombo[comboIndex].BeginAttack();
        }

        void HitEnd()
        {
            foreach (MeleeWeapon item in meleeWeaponCombo)
            {
                item.EndAttack();
            }
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, targettingRadius);
        }
#endif
    }
}