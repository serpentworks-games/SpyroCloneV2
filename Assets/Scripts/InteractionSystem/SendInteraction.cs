using UnityEngine;

namespace ScalePact.InteractionSystem
{
    [SelectionBase]
    public class SendInteraction : MonoBehaviour
    {
        public InteractionCommandType interactionType;
        public InteractionReceiver interactionReceiver;
        public bool isOneShot = false;
        public float coolDown = 1;

        float lastSendTime;
        bool isTriggered = false;

        [ContextMenu("Send Interaction")]
        public void Send()
        {
            if (isOneShot && isTriggered) return;
            if (Time.time - lastSendTime < coolDown) return;

            isTriggered = true;
            lastSendTime = Time.time;

            interactionReceiver.ReceiveCommand(interactionType);
        }

        protected virtual void Reset()
        {
            interactionReceiver = GetComponent<InteractionReceiver>();
        }

    }
}