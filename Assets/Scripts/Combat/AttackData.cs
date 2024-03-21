using UnityEngine;

namespace ScalePact.Combat
{
    [System.Serializable]
    public class AttackData
    {
        [field: SerializeField] public AttackName AttackName { get; private set; }
        [field: SerializeField] public float TransitionDuration { get; private set; }
        [field: SerializeField] public int ComboStateIndex { get; private set; } = -1;
        [field: SerializeField] public float ComboBlendTime { get; private set; }
        [field: SerializeField] public float AttackForce { get; private set; }
        [field: SerializeField] public float AttackForceBlendTime { get; private set; }
        [field: SerializeField] public DamageHandler DamageHandler { get; private set; }
        [field: SerializeField] public float KnockBackForce { get; private set; }
    }

    [System.Serializable]
    public enum AttackName
    {
        LightAttack1,
        LightAttack2,
        LightAttack3
    }
}