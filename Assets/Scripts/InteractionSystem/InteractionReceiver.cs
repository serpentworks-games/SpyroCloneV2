using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.InteractionSystem
{
    public class InteractionReceiver : MonoBehaviour
    {
        Dictionary<InteractionCommandType, List<System.Action>> handlers = new();

        public void ReceiveCommand(InteractionCommandType commandType)
        {
            List<System.Action> callbacks = null;
            if (handlers.TryGetValue(commandType, out callbacks))
            {
                foreach (var item in callbacks) item();
            }
        }

        public void Register(InteractionCommandType type, InteractionHandler handler)
        {
            List<System.Action> callbacks = null;

            if (!handlers.TryGetValue(type, out callbacks))
            {
                callbacks = handlers[type] = new List<System.Action>();
            }
            callbacks.Add(handler.OnInteraction);
        }

        public void Remove(InteractionCommandType type, InteractionHandler handler)
        {
            handlers[type].Remove(handler.OnInteraction);
        }
    }
}