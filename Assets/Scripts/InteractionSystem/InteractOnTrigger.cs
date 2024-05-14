using ScalePact.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace ScalePact.InteractionSystem
{
    [RequireComponent(typeof(Collider))]
    public class InteractOnTrigger : MonoBehaviour
    {
        [SerializeField] LayerMask triggerableLayers;
        [SerializeField] UnityEvent OnEnter, OnExit;

        new Collider collider;

        private void Reset()
        {
            triggerableLayers = LayerMask.NameToLayer("Everything");
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (triggerableLayers.Contains(other.gameObject))
            {
                ExecuteOnEnter(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (triggerableLayers.Contains(other.gameObject))
            {
                ExecuteOnExit(other);
            }
        }

        protected virtual void ExecuteOnEnter(Collider other)
        {
            OnEnter?.Invoke();
        }

        protected virtual void ExecuteOnExit(Collider other)
        {
            OnExit?.Invoke();
        }
    }
}