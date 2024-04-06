using System;
using UnityEngine;

namespace ScalePact.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 10;
        [SerializeField] bool isImmortal = false;

        public bool IsDead => currentHealth == 0;

        int currentHealth;

        public event Action OnReceiveDamage;
        public event Action OnDeath;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void ApplyDamage(int damage)
        {   
            if (currentHealth == 0) return;

            OnReceiveDamage?.Invoke();
            
            if (isImmortal) return;

            currentHealth = Mathf.Max(currentHealth - damage, 0);
            
            if (currentHealth == 0)
            {
                OnDeath?.Invoke();
            }

            Debug.Log($"{this.name}'s Health: {currentHealth} / {maxHealth}");
        }
    }
}