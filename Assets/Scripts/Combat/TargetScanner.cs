using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using ScalePact.Core;
using ScalePact.Core.Input;
using UnityEngine;

namespace ScalePact.Combat
{
    public class TargetScanner : MonoBehaviour
    {
        [SerializeField] float targettingRadius;
        [SerializeField] LayerMask targettingLayer;

        public Collider ActiveTarget { get; private set; }
        public List<Collider> TargetColliders { get; private set; } = new();

        Collider[] overlappedColliders;

        int targetIndex;
        bool isLockedOn;

        GameObject targetFollow;
        CinemachineTargetGroup targetGroup;

        InputManager inputManager;

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            targetGroup = FindObjectOfType<CinemachineStateDrivenCamera>().GetComponentInChildren<CinemachineTargetGroup>();
        }

        private void OnEnable()
        {
            inputManager.ToggleTargetEvent += OnLockOnTarget;
            inputManager.SwitchTargetEvent += OnSwitchTarget;
        }

        private void OnDisable()
        {
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

        public Collider GetClosestTargetNoTargetting()
        {
            return GetClosestTarget();
        }

        private void GetPotentialTargets()
        {
            overlappedColliders = Physics.OverlapSphere(transform.position, targettingRadius, targettingLayer, QueryTriggerInteraction.Ignore);
            TargetColliders.Clear();

            for (int i = 0; i < overlappedColliders.Length; i++)
            {
                if (overlappedColliders[i].TryGetComponent<Target>(out _))
                {
                    TargetColliders.Add(overlappedColliders[i]);
                }
            }
        }

        void SortTargetList()
        {
            TargetColliders = TargetColliders.OrderBy(x => Camera.main.WorldToScreenPoint(x.transform.position).x).ToList();
        }

        Collider GetClosestTarget()
        {
            if (TargetColliders.Count == 0) return null;

            Collider bestTarget = null;
            TargetColliders = TargetColliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
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

        #region Events Callbacks
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, targettingRadius);
        }
#endif
    }
}