using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] bool shouldInterpolateRotation = false;
    [SerializeField] float moveSpeed = 4;

    NavMeshAgent agent;
    Rigidbody rigidbody;

    bool shouldFollowAgent;
    bool isGrounded;
    bool isUnderExternalForces;
    bool shouldExternalForceAddGravity = true;
    Vector3 externalForce;

    const float kGroundedRayDistance = 0.8f;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        shouldFollowAgent = true;
    }

    private void FixedUpdate()
    {
        CheckIfGrounded();

        if (isUnderExternalForces)
        {
            ForceMovement();
            return;
        }
        else
        {
            if (shouldFollowAgent)
            {
                agent.speed = moveSpeed;
                transform.position = agent.nextPosition;
            }
            else
            {
                //animator things
            }
        }
    }

    public float GetAgentVelocity()
    {
        return agent.velocity.magnitude;
    }

    public void SetShouldFollowAgent(bool shouldFollow)
    {
        if (!shouldFollow && agent.enabled)
        {
            agent.ResetPath();
        }
        else if (shouldFollow && !agent.enabled)
        {
            agent.Warp(transform.position);
        }

        shouldFollowAgent = shouldFollow;
        agent.enabled = shouldFollow;
    }

    public void AddForce(Vector3 force, bool useGravity = true)
    {
        if (agent.enabled) agent.ResetPath();

        externalForce = force;
        agent.enabled = false;
        isUnderExternalForces = true;
        shouldExternalForceAddGravity = useGravity;
    }

    public void ClearForce()
    {
        isUnderExternalForces = false;
        agent.enabled = true;
    }

    public void SetForward(Vector3 forward)
    {
        Quaternion targetRot = Quaternion.LookRotation(forward);

        if (shouldInterpolateRotation)
        {
            targetRot = Quaternion.RotateTowards(transform.rotation, targetRot, agent.angularSpeed * Time.deltaTime);
        }

        transform.rotation = targetRot;
    }

    public bool SetTarget(Vector3 position)
    {
        return agent.SetDestination(position);
    }

    private void CheckIfGrounded()
    {
        Ray ray = new Ray(transform.position + 0.5f * kGroundedRayDistance * Vector3.up, -Vector3.up);
        isGrounded = Physics.Raycast(ray, out RaycastHit hit, kGroundedRayDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore);
    }

    private void ForceMovement()
    {
        if (shouldExternalForceAddGravity)
        {
            externalForce += Physics.gravity * Time.deltaTime;
        }

        Vector3 movement = externalForce * Time.deltaTime;

        if (!rigidbody.SweepTest(movement.normalized, out RaycastHit hit, movement.sqrMagnitude))
        {
            rigidbody.MovePosition(rigidbody.position + movement);
        }

        agent.Warp(rigidbody.position);
    }
}