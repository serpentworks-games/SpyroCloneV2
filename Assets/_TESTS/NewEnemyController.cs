using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyController : MonoBehaviour
{
    [SerializeField] EnemySO enemyConfigSO = null;
    [SerializeField] AttackRadius meleeAttackRadius;

    int currentHealth;
    Coroutine lookAtTarget;

    NavMeshAgent agent;
    NewEnemyMovement movement;
    Animator animator;

    private void Awake() {
        movement = GetComponent<NewEnemyMovement>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        meleeAttackRadius.OnAttack += OnAttack;
    }

    private void OnAttack(IDamageable target)
    {
        animator.SetTrigger("Attack");

        if(lookAtTarget != null)
        {
            StopCoroutine(lookAtTarget);
        }

        lookAtTarget = StartCoroutine(LookAt(target.GetDamageableTransform()));
    }

    private IEnumerator LookAt(Transform target)
    {
        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;

        while(time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, time);

            time += Time.deltaTime * 2;
            yield return null;
        }
        transform.rotation = lookRot;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupEnemyFromConfig();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupEnemyFromConfig()
    {
        currentHealth = enemyConfigSO.maxHealth;

        agent.speed = enemyConfigSO.agentSpeed;
        agent.angularSpeed = enemyConfigSO.agentAngularSpeed;
        agent.acceleration = enemyConfigSO.agentAcceleration;
        agent.stoppingDistance = enemyConfigSO.agentStoppingDistance;
        agent.radius = enemyConfigSO.agentRadius;
        agent.height = enemyConfigSO.agentHeight;
        agent.areaMask = enemyConfigSO.agentAreaMask;
        agent.avoidancePriority = enemyConfigSO.agentAvoidancePriority;
        agent.obstacleAvoidanceType = enemyConfigSO.obstacleAvoidanceType;

        movement.destinationUpdateSpeed = enemyConfigSO.destinationUpdateSpeed;

    }
}
