#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.Utils
{
    /// <summary>
    /// Allows for the visualization of box and sphere colliders in the Editor
    /// </summary>
    public class TriggerVolumeVisualization : MonoBehaviour
    {
        [SerializeField] Color volumeColor = Color.white;

        private void OnDrawGizmos()
        {
            Matrix4x4 rotMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

            Gizmos.matrix = rotMatrix;

            Gizmos.color = volumeColor;

            if (GetComponent<BoxCollider>())
            {
                BoxCollider collider = GetComponent<BoxCollider>();
                Gizmos.DrawCube(collider.center, collider.size);
            }
            else if (GetComponent<SphereCollider>())
            {
                SphereCollider collider = GetComponent<SphereCollider>();
                Gizmos.DrawSphere(collider.center, collider.radius);
            }
            else
            {
                return;
            }
        }
    }
}
#endif