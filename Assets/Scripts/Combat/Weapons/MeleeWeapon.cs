using System;
using ScalePact.Core;
using ScalePact.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace ScalePact.Combat
{
    public class MeleeWeapon : MonoBehaviour
    {
        public int damage = 1;
        public LayerMask targetLayers;
        public AttackPoint[] attackpoints = new AttackPoint[0];
        public GameObject owner;

        [System.Serializable]
        public class AttackPoint
        {
            public float radius;
            public Vector3 offset;
            public Transform attackRoot;
        }

        Vector3[] prevPos = null;
        Vector3 direction;
        bool isInAttack = false;

        static RaycastHit[] hitCache = new RaycastHit[32];
        static Collider[] colliderCache = new Collider[32];

        public void BeginAttack()
        {
            isInAttack = true;
            prevPos = new Vector3[attackpoints.Length];

            for (int i = 0; i < attackpoints.Length; i++)
            {
                Vector3 worldPos = attackpoints[i].attackRoot.position +
                                attackpoints[i].attackRoot.TransformVector(attackpoints[i].offset);
                prevPos[i] = worldPos;
            }
        }

        public void EndAttack()
        {
            isInAttack = false;
        }

        private void FixedUpdate()
        {
            if (isInAttack)
            {
                for (int i = 0; i < attackpoints.Length; i++)
                {
                    AttackPoint point = attackpoints[i];

                    Vector3 worldPos = point.attackRoot.position +
                        point.attackRoot.TransformVector(point.offset);
                    Vector3 attackVector = worldPos - prevPos[i];

                    if (attackVector.magnitude < 0.001f)
                    {
                        attackVector = Vector3.forward * 0.0001f;
                    }

                    Ray ray = new Ray(worldPos, attackVector.normalized);

                    int contacts = Physics.SphereCastNonAlloc(
                        ray, point.radius, hitCache, attackVector.magnitude,
                        ~0, QueryTriggerInteraction.Ignore);

                    for (int k = 0; k < contacts; k++)
                    {
                        Collider col = hitCache[k].collider;
                        if (col != null)
                        {
                            TryApplyDamage(col, point);
                        }
                    }

                    prevPos[i] = worldPos;
                }
            }
        }

        private void TryApplyDamage(Collider other, AttackPoint point)
        {

            Damageable d = other.GetComponent<Damageable>();

            if (d == null) return;

            if (d.gameObject == owner) return;
            if (!targetLayers.Contains(other.gameObject)) return;

            Damageable.DamageMessage data;

            data.damageAmount = damage;
            data.damager = this;
            data.damageFromDirection = direction.normalized;
            data.damageSource = owner.transform.position;
            data.throwing = false;
            data.shouldStopCamera = false;

            d.ApplyDamage(data);
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            for (int i = 0; i < attackpoints.Length; i++)
            {
                AttackPoint point = attackpoints[i];

                if (point.attackRoot != null)
                {
                    Vector3 worldPos = point.attackRoot.TransformVector(point.offset);
                    Gizmos.color = new Color(1, 1, 1, 0.4f);
                    Gizmos.DrawSphere(point.attackRoot.position + worldPos, point.radius);
                }
            }
        }
#endif
    }
}