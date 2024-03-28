using UnityEngine;

namespace ScalePact.Forces
{
    public class PlayerForceReceiver : ForceReceiver
    {
        private void FixedUpdate()
        {
            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, Drag);

            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
            }
        }

        public override void AddForce(Vector3 forceToAdd)
        {
            impact += forceToAdd;
        }
    }
}
