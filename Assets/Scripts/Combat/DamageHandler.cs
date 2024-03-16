using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.Combat
{
    public class DamageHandler : MonoBehaviour
    {
        [SerializeField] Collider rootCollider;

        List<Collider> alreadyCollidedWith = new();

        Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();

            DisableCollider();
        }

        public void EnableCollider()
        {
            collider.enabled = true;
            alreadyCollidedWith.Clear();
        }

        public void DisableCollider()
        {
            collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other == rootCollider) return;

            if (alreadyCollidedWith.Contains(other)) return;

            Debug.Log("Adding collider");

            alreadyCollidedWith.Add(other);

            Debug.Log("Trying to apply damage...");

            if(other.TryGetComponent<Health>(out Health health))
            {
                Debug.Log("Applying damage");
                health.ApplyDamage(1);
            }
        }
    }
}