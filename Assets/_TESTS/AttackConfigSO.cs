using UnityEngine;

[CreateAssetMenu(fileName = "Attack Config", menuName = "ScalePact/New Attack Config", order = 0)]
public class AttackConfigSO : ScriptableObject {
    public AttackType attackType = AttackType.Melee;
    public int attackDamage = 1;
    public float attackRange = 2f;
    public float attackSpeed = 1.5f;
}