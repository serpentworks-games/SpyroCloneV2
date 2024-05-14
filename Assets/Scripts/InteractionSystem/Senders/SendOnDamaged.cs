using ScalePact.Core;
using UnityEngine;

namespace ScalePact.InteractionSystem.Senders
{
    [RequireComponent(typeof(Health))]
    public class SendOnDamaged : SendInteraction
    {
        Health objHealth;

        private void Awake()
        {
            objHealth = GetComponent<Health>();
        }

        private void OnEnable() {
            objHealth.OnDeath += SendWhenDeathTriggered;
        }

        private void OnDisable() {
            objHealth.OnDeath -= SendWhenDeathTriggered;
        }

        void SendWhenDeathTriggered()
        {
            Send();
        }

    }
}
