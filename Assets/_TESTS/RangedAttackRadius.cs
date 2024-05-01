using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttackRadius : AttackRadius
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Vector3 spawnOffset = new Vector3(0, 1, 0);
    [SerializeField] LayerMask lineOfSightMask;
    [SerializeField] float sphereCastRadius = 0.1f;

    ObjectPool projectilePool;
    RaycastHit hit;
    IDamageable target;
    
    NavMeshAgent agent;

    protected override void Awake() {
        base.Awake();

        projectilePool = ObjectPool.CreateInstance(
            projectilePrefab, //Prefab to spawn
            Mathf.CeilToInt((1 / attackDelay) * projectilePrefab.autoDestroyTime) //How many to spawn based on attack speed and destroy time
            );
    }

    protected override IEnumerator Attack()
    {
        WaitForSeconds waitTime = new WaitForSeconds(attackDelay);
        yield return waitTime;

        while (damageables.Count > 0)
        {
            for (int i = 0; i < damageables.Count; i++)
            {
                if(HasLineOfSightToTarget(damageables[i].GetDamageableTransform()))
                {
                    target = damageables[i];
                    OnAttack?.Invoke(damageables[i]);
                    agent.enabled = false;
                    break;
                }
            }
        }
    }

    private bool HasLineOfSightToTarget(Transform target)
    {
        Vector3 raycastOrigin = transform.position + spawnOffset;
        Vector3 raycastDirection = ((target.position + spawnOffset) - (transform.position + spawnOffset)).normalized;
        if (Physics.SphereCast(raycastOrigin, sphereCastRadius, raycastDirection, out hit, collider.radius, lineOfSightMask))
        {
            return hit.collider.GetComponent<IDamageable>() != null;
        }
        return false;
    }
}
