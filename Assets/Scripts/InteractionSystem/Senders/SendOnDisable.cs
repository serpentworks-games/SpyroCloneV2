using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public class SendOnDisable : SendInteraction
    {
        private void OnDisable()
        {
            Send();
        }
    }
}