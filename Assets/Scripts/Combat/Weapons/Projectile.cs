using ScalePact.Utils;
using UnityEngine;

namespace ScalePact.Combat
{
    public abstract class Projectile : MonoBehaviour, IPooled<Projectile>
    {
        public int poolID { get; set; }
        public ObjectPooler<Projectile> pooler { get; set; }

        public abstract void Fire(Vector3 target, RangeWeapon shooter);
    }
}