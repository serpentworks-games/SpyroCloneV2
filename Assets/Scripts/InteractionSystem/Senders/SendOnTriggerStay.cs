using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public class SendOnTriggerStay : TriggerInteraction
    {
        [SerializeField] LayerMask triggerableLayer;

        private void OnTriggerStay(Collider other)
        {
            if (triggerableLayer.Contains(other.gameObject))
            {
                Send();
            }
        }
    }
}