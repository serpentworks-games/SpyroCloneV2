using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public class SendOnEnable : SendInteraction
    {
        private void OnEnable() {
            Send();
        }
    }
}