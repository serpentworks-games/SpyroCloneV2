using System.Collections;
using System.Collections.Generic;
using ScalePact.Core;
using ScalePact.Forces;
using UnityEngine;

namespace ScalePact.Combat
{
    public class DamageHandler : MonoBehaviour
    {
        [SerializeField] Collider rootCollider;

        List<Collider> alreadyCollidedWith = new();

        new Collider collider;

        float knockBackForce;

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

        public void SetUpAttack(float knockBackForce)
        {
            this.knockBackForce = knockBackForce;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == rootCollider) return;

            if (alreadyCollidedWith.Contains(other)) return;

            alreadyCollidedWith.Add(other);

            if (other.TryGetComponent<Health>(out Health health))
            {
                health.ApplyDamage(1);
            }

            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 knockBackVector = other.transform.position - collider.transform.position;
                forceReceiver.AddForce(knockBackVector * knockBackForce);
            }
        }
    }
}