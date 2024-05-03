using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : PoolableObject
{
    public float autoDestroyTime = 5f;
    [SerializeField] float projectileMoveSpeed = 2f;
    [SerializeField] int projectileDamage = 1;

    [HideInInspector] public Rigidbody rigidbody;

    const string kDisableMethodName = "DisableProjectile";

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        CancelInvoke(kDisableMethodName);
        Invoke(kDisableMethodName, autoDestroyTime);
    }

    private void OnTriggerEnter(Collider other) {
        IDamageable damageable;

        if(other.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.TakeDamage(projectileDamage);
        }

        DisableProjectile();
    }

    private void DisableProjectile()
    {
        CancelInvoke(kDisableMethodName);
        rigidbody.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    public float GetMoveSpeed()
    {
        return projectileMoveSpeed;
    }
}
