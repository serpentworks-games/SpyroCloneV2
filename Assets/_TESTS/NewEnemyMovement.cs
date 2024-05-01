using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [HideInInspector] public float destinationUpdateSpeed = 0.1f;

    NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    
    private void Start() {
        StartMovement();
    }

    public void StartMovement()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds waitTime = new WaitForSeconds(destinationUpdateSpeed);

        while(enabled)
        {
            agent.SetDestination(target.transform.position);
            yield return waitTime;
        }
    }
}
