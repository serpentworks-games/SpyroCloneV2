using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    public abstract class TriggerInteraction : SendInteraction
    {
        const string kLayerToResetTo = "Environment";
        protected override void Reset()
        {
            if (LayerMask.LayerToName(gameObject.layer) == "Default")
            {
                gameObject.layer = LayerMask.NameToLayer(kLayerToResetTo);
            }

            var col = GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = true;
            }
            
            base.Reset();
        }
    }
}