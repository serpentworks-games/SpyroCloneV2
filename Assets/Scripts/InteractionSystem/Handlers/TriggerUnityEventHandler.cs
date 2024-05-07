using UnityEngine;
using UnityEngine.Events;

namespace ScalePact.InteractionSystem.Handlers
{
    public class TriggerUnityEventHandler : InteractionHandler
    {
        [SerializeField] UnityEvent unityEvent;

        public override void PerformInteraction()
        {
            unityEvent?.Invoke();
        }
    }
}