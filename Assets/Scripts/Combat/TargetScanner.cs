using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace ScalePact.Combat
{
    public class TargetScanner : MonoBehaviour
    {
        [field: SerializeField] public float TargetRadius { get; private set; } = 10f;
        [field: SerializeField] public CinemachineTargetGroup TargetGroup { get; private set; }

        public Target CurrentTarget { get; private set; }

        Camera mainCamera;

        List<Target> targets = new();

        private void Start() {
            mainCamera = Camera.main;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out Target candidateTarget)) return;
            targets.Add(candidateTarget);
            candidateTarget.OnTargetDestroyed += RemoveTargetFromTargetGroup;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out Target candidateTarget)) return;

            targets.Remove(candidateTarget);
            RemoveTargetFromTargetGroup(candidateTarget);
            candidateTarget.OnTargetDestroyed -= RemoveTargetFromTargetGroup;
        }

        public bool SelectTarget()
        {
            if (targets.Count == 0) return false; //if no targets, don't try to find one

            Target closestTarget = null;
            float closestTargetDistanceToCenter = Mathf.Infinity;
            
            foreach (Target target in targets)
            {
                Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
                if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
                {
                    continue;
                }

                Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);

                if(toCenter.sqrMagnitude < closestTargetDistanceToCenter)
                {
                    closestTarget = target;
                    closestTargetDistanceToCenter = toCenter.sqrMagnitude;
                }
            }

            if (closestTarget == null) return false;

            CurrentTarget = closestTarget;
            TargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
            
            return true;
        }

        public void ClearCurrentTarget()
        {
            RemoveTargetFromTargetGroup(CurrentTarget);
        }

        private void RemoveTargetFromTargetGroup(Target target)
        {
            if (CurrentTarget == target)
            {
                TargetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
            }
            target.OnTargetDestroyed -= RemoveTargetFromTargetGroup;
        }
    }
}