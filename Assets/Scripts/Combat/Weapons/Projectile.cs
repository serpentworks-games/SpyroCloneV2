using System;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Combat
{
    public class Projectile : MonoBehaviour, IPooled<Projectile>
    {
        //Object Pooler Properties
        public int poolID { get; set; }
        public ObjectPooler<Projectile> pooler { get; set; }

        public enum ProjectileType
        {

        }

        public ProjectileType type;
        public float projectileSpeed;
        public int damageAmount = 1;
        public float knockBackForce;
        public float explosionRadius;
        public float explosionTimer;
        public LayerMask damageableMask;

        protected float timeSinceFired = Mathf.Infinity;
        protected RangeWeapon shooter;
        protected Rigidbody rb;

        protected static Collider[] explosionHitCache = new Collider[32];

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.detectCollisions = false;
        }

        private void OnEnable()
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.isKinematic = true;
        }

        private void FixedUpdate()
        {
            timeSinceFired += Time.deltaTime;

            //allows the projectile to get past the shooter's collider
            if (timeSinceFired > 0.2f) rb.detectCollisions = true;

            if (explosionTimer > 0 && timeSinceFired > explosionTimer) Explode();
        }

        public virtual void Explode()
        {
            int count = Physics.OverlapSphereNonAlloc(
                transform.position, explosionRadius, explosionHitCache, damageableMask.value);

            Damageable.DamageMessage msg = new()
            {
                damageAmount = this.damageAmount,
                damageSource = transform.position,
                damager = this,
                knockBackForce = knockBackForce

            };

            for (int i = 0; i < count; i++)
            {
                Damageable d = explosionHitCache[i].GetComponentInChildren<Damageable>();

                if (d != null) d.ApplyDamage(msg);
            }

            pooler.FreeInstance(this);
        }

        public virtual void Fire(Vector3 target, RangeWeapon shooter)
        {
            rb.isKinematic = false;
            this.shooter = shooter;

            rb.velocity = GetVelocity(target);
            rb.AddRelativeTorque(Vector3.right * -5500.0f);
            rb.detectCollisions = false;
            transform.forward = target - transform.position;
        }

        public virtual void OnCollisionEnter(Collision other)
        {

        }

        private Vector3 GetVelocity(Vector3 target)
        {
            Vector3 velocity = Vector3.zero;
            Vector3 toTarget = target - transform.position;

            float gravitySquared = Physics.gravity.sqrMagnitude;
            float b = projectileSpeed * projectileSpeed + Vector3.Dot(toTarget, Physics.gravity);
            float discriminant = b * b - gravitySquared * toTarget.sqrMagnitude;

            if (discriminant < 0)
            {
                velocity = toTarget;
                velocity.y = 0;
                velocity.Normalize();
                velocity.y = 0.7f;

                velocity *= projectileSpeed;
                return velocity;
            }

            float discRoot = Mathf.Sqrt(discriminant);

            float directShot = Mathf.Sqrt((b - discRoot) * 2f / gravitySquared);

            velocity = toTarget / directShot - Physics.gravity * directShot / 2f;

            return velocity;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
#endif
    }
}