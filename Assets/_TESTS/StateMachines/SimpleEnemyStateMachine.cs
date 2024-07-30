using ScalePact.Combat;
using ScalePact.Core;
using ScalePact.Utils;
using UnityEngine;

public class SimpleEnemyStateMachine : StateMachine, IMessageReceiver
{
    [SerializeField] EnemyTargetScanner playerScanner;
    [SerializeField] float suspicionStateTime = 4f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float impactDuraction = 0.5f;
    [SerializeField] float forceMultiplierOnHit = 5.5f;
    [SerializeField] float forceMultiplierOnDeath = 7f;

    public float testWaitTimeForAttack = 1f;

    public bool IsGrounded { get; set; }
    public Health Target { get => currentTarget; }
    public float SuspicionStateTime { get => suspicionStateTime; }
    public float AttackRange { get => attackRange; }
    public EnemyMovementNEW Movement { get => movement; }
    public TargetDistributor.TargetFollower FollowerData { get => followerInstance; }
    public Animator Animator { get => animator; }
    public float ImpactDuration { get => impactDuraction; }

    Vector3 originalPosition;
    Health currentTarget = null;
    TargetDistributor.TargetFollower followerInstance = null;

    Animator animator;
    EnemyMovementNEW movement;

    private void OnEnable()
    {
        playerScanner.FindPlayer();

        movement = GetComponent<EnemyMovementNEW>();
        animator = GetComponent<Animator>();

        originalPosition = transform.position;

        SwitchState(new SimpleEnemyIdleState(this));
    }

    public override void Update()
    {
        base.Update();

        animator.SetFloat("Velocity", movement.GetAgentVelocity(), 0.1f, Time.deltaTime);
    }

    public void FindTarget()
    {
        Health newTarget = playerScanner.Detect(transform, currentTarget == null);

        if (currentTarget == null)
        {
            if (newTarget != null)
            {
                currentTarget = newTarget;
                TargetDistributor distributor = Target.GetComponent<TargetDistributor>();

                if (distributor != null)
                {
                    followerInstance = distributor.RegisterNewFollower();
                }
            }
        }
        else
        {
            if (newTarget == null)
            {
                Vector3 toTarget = currentTarget.transform.position - transform.position;
                if (toTarget.sqrMagnitude > playerScanner.DetectionRadius * playerScanner.DetectionRadius)
                {
                    if (followerInstance != null)
                    {
                        followerInstance.distributor.UnregisterFollower(followerInstance);
                    }

                    currentTarget = null;
                }
            }
            else
            {
                if (newTarget != currentTarget)
                {
                    if (followerInstance != null)
                    {
                        followerInstance.distributor.UnregisterFollower(followerInstance);
                    }

                    currentTarget = newTarget;

                    TargetDistributor distributor = Target.GetComponent<TargetDistributor>();
                    if (distributor != null)
                    {
                        followerInstance = distributor.RegisterNewFollower();
                    }
                }
            }
        }
    }

    public void RequestTargetPosition()
    {
        Vector3 fromTarget = transform.position - currentTarget.transform.position;
        fromTarget.y = 0;

        followerInstance.requiredPosition = currentTarget.transform.position + fromTarget.normalized * attackRange * 0.9f;
    }

    public void ReturnToOriginalPosition()
    {
        if (followerInstance != null)
        {
            followerInstance.distributor.UnregisterFollower(followerInstance);
        }

        currentTarget = null;
        movement.SetTarget(originalPosition);
    }

    #region SwitchTo State Functions
    public void SwitchToIdleState()
    {
        animator.CrossFadeInFixedTime("Movement", 0.1f);
        SwitchState(new SimpleEnemyIdleState(this));
    }

    public void SwitchToChaseState()
    {
        if (followerInstance != null)
        {
            followerInstance.requireAttackSlot = true;
            RequestTargetPosition();
        }

        SwitchState(new SimpleEnemyChaseState(this));
    }

    public void SwitchToAttackState()
    {
        animator.CrossFadeInFixedTime("Attack", 0.1f);
        SwitchState(new SimpleEnemyAttackState(this));
    }

    public void SwitchToSuspicionState()
    {
        if (followerInstance != null)
        {
            followerInstance.requireAttackSlot = false;
        }

        SwitchState(new SimpleEnemySuspicionState(this));
    }

    public void SwitchToDeathState(Damageable.DamageMessage msg)
    {
        SwitchState(new SimpleEnemyDeathState(this));
    }

    public void SwitchToImpactState(Damageable.DamageMessage msg)
    {
        animator.CrossFadeInFixedTime("Impact", 0.1f);
        SwitchState(new SimpleEnemyHitState(this));
    }


    #endregion


    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        switch (type)
        {
            case MessageType.DEAD:
                SwitchToDeathState((Damageable.DamageMessage)msg);
                break;
            case MessageType.DAMAGED:
                SwitchToImpactState((Damageable.DamageMessage)msg);
                break;

        }
    }

    private void ApplyForces(Damageable.DamageMessage msg, float multiplier)
    {
        Vector3 pushForce = transform.position - msg.damageSource;

        pushForce.y = 0;

        transform.forward = -pushForce.normalized;
        movement.AddForce(pushForce.normalized * multiplier, false);
    }

    private void OnDrawGizmos()
    {
        playerScanner.EditorGizmo(transform);
    }
}