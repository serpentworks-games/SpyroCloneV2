using System;
using UnityEngine;

namespace ScalePact.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 10;

        public bool IsDead => currentHealth == 0;

        int currentHealth;

        public event Action OnReceiveDamage;
        public event Action OnDeath;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        private void Update()
        {

        }

        public void ApplyDamage(int damage)
        {
            if (currentHealth == 0) return;

            currentHealth = Mathf.Max(currentHealth - damage, 0);
            OnReceiveDamage?.Invoke();

            if (currentHealth == 0)
            {
                OnDeath?.Invoke();
            }

            Debug.Log($"{this.name}'s Health: {currentHealth} / {maxHealth}");
        }
    }
}