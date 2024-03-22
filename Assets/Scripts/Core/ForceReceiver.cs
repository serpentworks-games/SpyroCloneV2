using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Forces
{
    public abstract class ForceReceiver : MonoBehaviour
    {
        [field: Header("Baseline Variables")]
        [field: SerializeField] public float Drag { get; private set; }
        [field: SerializeField] public bool UsesRigidBody { get; private set; } = false;
        //public properties
        public Vector3 Movement => impact + Vector3.up * verticalVelocity;

        protected Vector3 impact;
        protected float verticalVelocity;
        protected Vector3 dampingVelocity;

        public abstract void AddForce(Vector3 forceToAdd);

        public abstract bool IsGrounded();

    }
}