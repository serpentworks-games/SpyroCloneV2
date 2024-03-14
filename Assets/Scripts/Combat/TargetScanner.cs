using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.Combat
{
    public class TargetScanner : MonoBehaviour
    {
        public List<Target> targets = new();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Target>(out Target candidateTarget)) return;
            targets.Add(candidateTarget);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Target>(out Target candidateTarget)) return;
            
            targets.Remove(candidateTarget);
        }
    }
}