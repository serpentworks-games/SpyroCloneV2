using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public class SendOnTriggerExit : TriggerInteraction
    {
        [SerializeField] LayerMask triggerableLayer;

        private void OnTriggerExit(Collider other)
        {
            if (triggerableLayer.Contains(other.gameObject))
            {
                Send();
            }
        }
    }
}