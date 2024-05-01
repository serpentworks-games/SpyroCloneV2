using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTarget : MonoBehaviour, IDamageable
{
    public int health = 10;
    public Transform GetDamageableTransform()
    {
        return transform;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            health = 0;
            gameObject.SetActive(false);
        }
    }

}
