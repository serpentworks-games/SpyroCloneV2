using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollow : MonoBehaviour
{
    [SerializeField] Transform targetToFollow;

    private void LateUpdate()
    {
        transform.SetPositionAndRotation(targetToFollow.position, targetToFollow.rotation);
    }
}
