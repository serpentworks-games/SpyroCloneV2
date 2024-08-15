using System;
using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Combat
{
    public class RangeWeapon : MonoBehaviour
    {
        [SerializeField] Vector3 launchOffset;
        [SerializeField] Projectile projectilePrefab;
        [SerializeField] int startingPoolCount;

        public Projectile LoadedProjectile { get => loadedProjectile; }

        Projectile loadedProjectile = null;
        ObjectPooler<Projectile> projectilePool;

        private void Start()
        {
            projectilePool = new();
            projectilePool.Init(startingPoolCount, projectilePrefab);
        }

        public void Attack(Vector3 target)
        {
            AttackWithProjectile(target);
        }

        public void LoadProjectile()
        {
            if (loadedProjectile != null) return;

            loadedProjectile = projectilePool.GetNewInstance();
            loadedProjectile.transform.SetParent(transform, false);
            loadedProjectile.transform.localPosition = launchOffset;
            loadedProjectile.transform.localRotation = Quaternion.identity;
        }

        private void AttackWithProjectile(Vector3 target)
        {
            if (loadedProjectile == null) LoadProjectile();

            loadedProjectile.transform.SetParent(null, true);
            loadedProjectile.Fire(target, this);
            loadedProjectile = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 worldOffset = transform.TransformPoint(launchOffset);
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawLine(worldOffset + Vector3.up * 0.4f, worldOffset + Vector3.down * 0.4f);
            UnityEditor.Handles.DrawLine(worldOffset + Vector3.forward * 0.4f, worldOffset + Vector3.back * 0.4f);
        }
#endif
    }
}