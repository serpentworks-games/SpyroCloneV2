
using System;
using UnityEngine;


namespace ScalePact.InteractionSystem
{
    /// <summary>
    /// Base class for behaviour that receives a command and does some sort of behaviour
    /// IE: switching materials, triggering and animation, etc
    /// </summary>
    [SelectionBase]
    [RequireComponent(typeof(InteractionReceiver))]
    public abstract class InteractionHandler : MonoBehaviour
    {
        public InteractionCommandType interactionType;
        public bool isOneShot = true;
        public float coolDown = 0;
        public float startDelay = 0;

        protected bool isTriggered = false;
        float startTime = 0f;

        protected virtual void Awake()
        {
            GetComponent<InteractionReceiver>().Register(interactionType, this);
        }

        public abstract void PerformInteraction();

        public virtual void OnInteraction()
        {
            if (isOneShot && isTriggered) return;

            isTriggered = true;

            if (coolDown > 0)
            {
                if (Time.time > startTime + coolDown)
                {
                    startTime = Time.time + startDelay;
                    ExecuteInteraction();
                }
            }
            else
            {
                ExecuteInteraction();
            }
        }

        private void ExecuteInteraction()
        {
            if (startDelay > 0)
            {
                Invoke("PerformInteraction", startDelay);
            }
            else
            {
                PerformInteraction();
            }
        }
    }
}