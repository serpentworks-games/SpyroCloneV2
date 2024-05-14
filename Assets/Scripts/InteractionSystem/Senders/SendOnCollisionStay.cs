using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public class SendOnCollisionStay : SendInteraction
    {
        [SerializeField] LayerMask collidableLayers;

        private void OnCollisionStay(Collision other)
        {
            if (collidableLayers.Contains(other.gameObject))
            {
                Send();
            }
        }
    }
}