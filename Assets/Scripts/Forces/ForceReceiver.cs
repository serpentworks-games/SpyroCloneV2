using UnityEngine;

namespace ScalePact.Forces
{
    public abstract class ForceReceiver : MonoBehaviour
    {
        [Header("Baseline Variables")]
        [SerializeField]protected float impactDrag;
        //public properties
        public Vector3 Movement => impact + Vector3.up * verticalVelocity;

        protected Vector3 impact;
        protected float verticalVelocity;
        protected Vector3 dampingVelocity;

        public abstract void AddForce(Vector3 forceToAdd);

    }
}