using UnityEngine;

namespace ScalePact.InteractionSystem
{
    public class TestHandler : InteractionHandler
    {
        public override void PerformInteraction()
        {
            Debug.Log("Triggering!");
        }
    }
}