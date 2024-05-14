using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public class SendOnCollisionEnter : SendInteraction
    {
        [SerializeField] LayerMask collidableLayers;

        private void OnCollisionEnter(Collision other)
        {
            if (collidableLayers.Contains(other.gameObject))
            {
                Send();
            }
        }
    }
}