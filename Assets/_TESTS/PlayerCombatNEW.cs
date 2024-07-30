using System.Collections;
using System.Collections.Generic;
using ScalePact.Core.Input;
using ScalePact.Utils;
using UnityEngine;

public class PlayerCombatNEW : MonoBehaviour
{
    [SerializeField] Weapon testWeapon;
    InputManager inputs;
    Animator animator;

    private void Awake() {
        inputs = GetComponent<InputManager>();
        animator = GetComponent<Animator>();

        inputs.LightAttackEvent += OnLightAttack;

    }

    private void OnDisable() {
        inputs.LightAttackEvent -= OnLightAttack;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLightAttack()
    {
        animator.SetTrigger(PlayerHashIDs.AttackTriggerHash);
    }

    void Hit()
    {
        testWeapon.BeginAttack();
    }

    void HitEnd()
    {
        testWeapon.EndAttack();
    }
}
