using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Config", menuName = "ScalePact/New Enemy Config", order = 0)]
public class EnemySO : ScriptableObject
{
    [Header("Basic Settings")]
    public int maxHealth = 3;
    public bool isRanged = false;
    

    [Header("NavMeshAgent Settings")]
    public float destinationUpdateSpeed = 0.1f;
    public float agentSpeed = 3.5f;
    public float agentAngularSpeed = 1200;
    public float agentAcceleration = 8;
    public float agentStoppingDistance = 2;
    public float agentRadius = 0.5f;
    public float agentHeight = 2f;
    [Tooltip("Int index of the area mask, -1 means everything")]
    public int agentAreaMask = -1;
    public int agentAvoidancePriority = 50;
    public ObstacleAvoidanceType obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;

}