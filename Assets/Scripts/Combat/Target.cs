using System;
using UnityEngine;

namespace ScalePact.Combat
{
    public class Target : MonoBehaviour
    {
        public event Action<Target> OnTargetDestroyed;

        private void OnDestroy() {
            OnTargetDestroyed?.Invoke(this);
        }
    }
}
