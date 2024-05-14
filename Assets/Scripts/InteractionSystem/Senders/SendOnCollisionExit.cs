using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public class SendOnCollisionExit : SendInteraction
    {
        [SerializeField] LayerMask collidableLayers;

        private void OnCollisionExit(Collision other)
        {
            if (collidableLayers.Contains(other.gameObject))
            {
                Send();
            }
        }
    }
}