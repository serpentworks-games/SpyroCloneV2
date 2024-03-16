using System;
using System.Collections;
using System.Collections.Generic;
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
