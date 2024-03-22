using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.Core
{
    public class Ragdoll : MonoBehaviour
    {
        Animator animator;
        CharacterController controller;

        Collider[] ragdollColliders;
        Rigidbody[] ragdollRigidBodies;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
        }

        void Start()
        {
            ragdollColliders = GetComponentsInChildren<Collider>(true);
            ragdollRigidBodies = GetComponentsInChildren<Rigidbody>(true);

            ToggleRagdoll(false);
        }

        public void ToggleRagdoll(bool isRagdoll)
        {
            foreach (Collider collider in ragdollColliders)
            {
                if (collider.gameObject.CompareTag("Ragdoll"))
                {
                    collider.enabled = isRagdoll;
                }
            }

            foreach (Rigidbody rigidbody in ragdollRigidBodies)
            {
                if (rigidbody.gameObject.CompareTag("Ragdoll"))
                {
                    rigidbody.isKinematic = !isRagdoll;
                    rigidbody.useGravity = isRagdoll;
                }
            }

            controller.enabled = !isRagdoll;
            animator.enabled = !isRagdoll;
        }
    }
}