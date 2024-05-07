using UnityEngine;

namespace ScalePact.InteractionSystem.Handlers
{
    public class ToggleGameObjectActive : InteractionHandler
    {
        [SerializeField] GameObject[] objectsToToggle;

        public override void PerformInteraction()
        {
            foreach (var obj in objectsToToggle)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }
}