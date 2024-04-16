using System;
using UnityEngine;

namespace ScalePact.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 10;
        [SerializeField] bool isImmortal = false;

        public bool IsDead => currentHealth == 0;

        int currentHealth;

        public event Action OnReceiveDamage;
        public event Action OnDeath;

        ActionScheduler actionScheduler;

        private void Awake()
        {
            currentHealth = maxHealth;
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public void ApplyDamage(int damage)
        {   
            if (currentHealth == 0) return;

            OnReceiveDamage?.Invoke();
            
            if (isImmortal) return;

            currentHealth = Mathf.Max(currentHealth - damage, 0);
            
            if (currentHealth == 0)
            {
                InvokeDeath();
            }

            Debug.Log($"{this.name}'s Health: {currentHealth} / {maxHealth}");
        }

        private void InvokeDeath()
        {
            OnDeath?.Invoke();

            if (actionScheduler != null)
            {
                actionScheduler.CancelCurrentAction();
            }
        }
    }
}