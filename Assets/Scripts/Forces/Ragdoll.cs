using UnityEngine;

namespace ScalePact.Forces
{
    public class Ragdoll : MonoBehaviour
    {
        Animator animator;

        Collider[] ragdollColliders;
        Rigidbody[] ragdollRigidBodies;

        private void Awake()
        {
            animator = GetComponent<Animator>();
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

            animator.enabled = !isRagdoll;
        }
    }
}