using ScalePact.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace ScalePact.InteractionSystem
{
    [RequireComponent(typeof(Collider))]
    public class InteractOnCollision : MonoBehaviour
    {
        [SerializeField] LayerMask collidableLayers;
        [SerializeField] UnityEvent OnCollision;

        private void Reset()
        {
            collidableLayers = LayerMask.NameToLayer("Everything");
        }

        private void OnCollisionEnter(Collision other)
        {
            if (collidableLayers.Contains(other.transform.gameObject))
            {
                ExecuteOnEnter(other);
            }
        }

        protected virtual void ExecuteOnEnter(Collision other)
        {
            OnCollision?.Invoke();
        }

    }
}