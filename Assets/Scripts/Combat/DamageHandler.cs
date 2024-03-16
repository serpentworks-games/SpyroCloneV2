using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.Combat
{
    public class DamageHandler : MonoBehaviour
    {
        Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            DisableCollider();
        }

        public void EnableCollider()
        {
            collider.enabled = true;
        }

        public void DisableCollider()
        {
            collider.enabled = false;
        }
    }
}