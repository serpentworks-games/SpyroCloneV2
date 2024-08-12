using ScalePact.Combat;
using ScalePact.Core;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    [RequireComponent(typeof(Damageable))]
    public class SendOnDamaged : SendInteraction, IMessageReceiver
    {
        public void OnReceiveMessage(MessageType type, object sender, object msg)
        {
            switch (type)
            {
                case MessageType.DAMAGED:
                    Send();
                    break;
                case MessageType.DEAD:
                    break;
            }
        }
    }
}
