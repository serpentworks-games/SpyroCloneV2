using UnityEngine;

namespace ScalePact.InteractionSystem.Handlers
{
    public class SwitchMaterialHandler : InteractionHandler
    {
        [SerializeField] Renderer objectToSwitch;
        [SerializeField] Material[] materialsToSwitchBtwn;

        int count;

        public override void PerformInteraction()
        {
            count++;
            objectToSwitch.material = materialsToSwitchBtwn[count % materialsToSwitchBtwn.Length];
        }
    }
}