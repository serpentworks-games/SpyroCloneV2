using System.Collections.Generic;
using ScalePact.Combat;
using ScalePact.Forces;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Core
{
    public class DamageHandler : MonoBehaviour
    {
        [SerializeField] Collider rootCollider;
        [SerializeField] LayerMask collidableLayers;

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
            // //If the collider is us, ignore it
            // if (other == rootCollider) return;

            // //If we've already collided with the collider, ignore it
            // if (alreadyCollidedWith.Contains(other)) return;

            // //If the collider is not on the collidable layers, ignore it
            // if (!LayerMaskExtensions.Contains(collidableLayers, other.gameObject)) return;

            // //Add the collider to the already collided with list
            // alreadyCollidedWith.Add(other);

            // //Try to apply damage
            // if (other.TryGetComponent(out Health health))
            // {
            //     health.ApplyDamage(1);
            // }

            // //Try to apply force
            // if (other.TryGetComponent(out ForceReceiver forceReceiver))
            // {
            //     Vector3 knockBackVector = other.transform.position - collider.transform.position;
            //     forceReceiver.AddForce(knockBackVector * knockBackForce);
            // }
        }

        bool TryToApplyDamage(Collider other)
        {
            Damageable d = other.GetComponent<Damageable>();
            if(d == null) return false;

            if(d.gameObject == rootCollider.gameObject) return true;

            if(collidableLayers.Contains(other.gameObject)) return false;

            return false;
        }
    }
}