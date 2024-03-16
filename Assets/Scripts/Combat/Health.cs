using UnityEngine;

namespace ScalePact.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth = 10;

        int currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void ApplyDamage(int damage)
        {
            if (currentHealth <= 0) return;

            currentHealth = Mathf.Max(currentHealth - damage, 0);
            Debug.Log($"{this.name}'s Health: {currentHealth} / {maxHealth}");
        }
    }
}