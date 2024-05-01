using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackRadius : MonoBehaviour {

    public float attackDelay = 1.5f;
    public int attackDamage = 1;

    protected List<IDamageable> damageables = new();

    public delegate void AttackEvent(IDamageable target);
    public AttackEvent OnAttack;

    [HideInInspector] public SphereCollider collider;

    protected Coroutine AttackCoroutine;

    protected virtual void Awake() {
        collider = GetComponent<SphereCollider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageables.Add(damageable);

            if (AttackCoroutine == null)
            {
                AttackCoroutine = StartCoroutine(Attack());
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageables.Remove(damageable);

            if (damageables.Count == 0)
            {
                StopCoroutine(Attack());
                AttackCoroutine = null;
            }
        }
    }

    protected virtual IEnumerator Attack()
    {
        WaitForSeconds waitTime = new WaitForSeconds(attackDelay);
        yield return waitTime;

        IDamageable closestTarget = null;
        float closestDistance = float.MaxValue;

        while(damageables.Count > 0)
        {
            for (int i = 0; i < damageables.Count; i++)
            {
                Transform targetTransform = damageables[i].GetDamageableTransform();
                float dist = Vector3.Distance(transform.position, targetTransform.position);

                if(dist < closestDistance)
                {
                    closestDistance = dist;
                    closestTarget = damageables[i];
                }
            }

            if(closestTarget != null)
            {
                OnAttack?.Invoke(closestTarget);
                closestTarget.TakeDamage(attackDamage);
            }

            closestTarget = null;
            closestDistance = float.MaxValue;

            yield return waitTime;

            damageables.RemoveAll(RemoveDisabled);
        }
        AttackCoroutine = null;
    }

    protected bool RemoveDisabled(IDamageable target)
    {
        return target != null && !target.GetDamageableTransform().gameObject.activeSelf;
    }
}