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
        [SerializeField] bool useWireFrame = false;

        private void OnDrawGizmos()
        {
            Matrix4x4 rotMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

            Gizmos.matrix = rotMatrix;

            Gizmos.color = volumeColor;

            if (GetComponent<BoxCollider>())
            {
                BoxCollider collider = GetComponent<BoxCollider>();
                if (useWireFrame)
                {
                    Gizmos.DrawWireCube(collider.center, collider.size);
                }
                else
                {
                    Gizmos.DrawCube(collider.center, collider.size);
                }
            }
            else if (GetComponent<SphereCollider>())
            {
                SphereCollider collider = GetComponent<SphereCollider>();
                if (useWireFrame)
                {
                    Gizmos.DrawWireSphere(collider.center, collider.radius);
                }
                else
                {
                    Gizmos.DrawSphere(collider.center, collider.radius);
                }
            }
            else
            {
                return;
            }
        }
    }
}
#endif