using System;
using System.Collections;
using ScalePact.AI;
using ScalePact.Combat;
using ScalePact.Core;
using ScalePact.Forces;
using ScalePact.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace ScalePact.Enemies
{
    public class EnemyController : MonoBehaviour
    {

        // void PatrolState()
        // {
        //     Vector3 nextPos = guardPosition;

        //     if (patrolArea != null)
        //     {
        //         nextPos = GetNextPointViaArea();
        //     }

        //     if (patrolPath != null)
        //     {
        //         nextPos = GetNextPointViaWaypoint();
        //     }

        //     if (timeSinceArrivedAtPatrolPoint > patrolPointDwellTime)
        //     {
        //         movement.StartMoveAction(nextPos, patrollingSpeedModifier);
        //     }
        // }

        // #region Patrol Areas
        // private Vector3 GetNextPointViaArea()
        // {
        //     if (AtRandomPoint())
        //     {
        //         timeSinceArrivedAtPatrolPoint = 0;
        //         GetNewRandomPoint();
        //     }
        //     return GetCurrentRandomPoint();
        // }

        // private bool AtRandomPoint()
        // {
        //     float distanceToPoint = Vector3.Distance(transform.position, GetCurrentRandomPoint());
        //     return distanceToPoint < patrolPointTolerance;
        // }

        // private void GetNewRandomPoint()
        // {
        //     patrolArea.GenerateRandomPoint();
        // }

        // private Vector3 GetCurrentRandomPoint()
        // {
        //     return patrolArea.GetGeneratedPoint();
        // }
        // #endregion

        // #region Patrol Paths
        // private Vector3 GetNextPointViaWaypoint()
        // {
        //     if (AtWaypoint())
        //     {
        //         timeSinceArrivedAtPatrolPoint = 0;
        //         GetNextWaypoint();
        //     }
        //     return GetCurrentWayPoint();
        // }

        // private bool AtWaypoint()
        // {
        //     float distanceToPoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
        //     return distanceToPoint < patrolPointTolerance;
        // }

        // private void GetNextWaypoint()
        // {
        //     currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        // }

        // private Vector3 GetCurrentWayPoint()
        // {
        //     return patrolPath.GetWaypoint(currentWaypointIndex);
        // }
        // #endregion
    }
}