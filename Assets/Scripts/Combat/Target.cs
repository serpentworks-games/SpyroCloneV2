using System;
using UnityEngine;

namespace ScalePact.Core
{
    [RequireComponent(typeof(Health))]
    public class Target : MonoBehaviour
    {
        public event Action<Target> OnTargetDestroyed;

        private void OnDestroy() {
            OnTargetDestroyed?.Invoke(this);
        }
    }
}
