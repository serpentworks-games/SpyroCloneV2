using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public class SendOnTriggerEnter : TriggerInteraction
    {
        [SerializeField] LayerMask triggerableLayer;

        private void OnTriggerEnter(Collider other) {
            if(triggerableLayer.Contains(other.gameObject))
            {
                Send();
            }    
        }
    }
}