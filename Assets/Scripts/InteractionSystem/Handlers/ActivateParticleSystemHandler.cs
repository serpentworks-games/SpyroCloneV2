using ScalePact.InteractionSystem;
using UnityEngine;

namespace ScalePact.InteractionSystem.Handlers
{
    public class ActivateParticleSystemHandler : InteractionHandler
    {
        [SerializeField] ParticleSystem[] particleSystems;
        [SerializeField] int particleEmitCount;
        [SerializeField] bool shouldEmitParticles;

        public override void PerformInteraction()
        {
            if(!shouldEmitParticles)
            {
                foreach (var system in particleSystems)
                {
                    system.Play();
                }
            }
            else
            {
                foreach (var system in particleSystems)
                {
                    system.Emit(particleEmitCount); 
                }
            }
        }
    }
}