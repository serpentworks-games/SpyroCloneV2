using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.AI
{
    public class PatrolPath : MonoBehaviour
    {
        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount) return 0;

            return i + 1;
        }


#if UNITY_EDITOR
        [Header("Editor Gizmo Colors")]
        [SerializeField] Color selectedColor = new Color(1, 1, 1, 1);
        [SerializeField] Color deselectedColor = new Color(1, 1, 1, 0.25f);

        const float kWaypointGizmoRadius = 0.5f;

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DrawGizmos(i, selectedColor);
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DrawGizmos(i, deselectedColor);
            }
        }

        private void DrawGizmos(int i, Color color)
        {
            //Set the gizmo color
            Gizmos.color = color;

            //Draw the waypoints
            Gizmos.DrawSphere(GetWaypoint(i), kWaypointGizmoRadius);

            //If there's only one, return early
            if (transform.childCount == 1) { return; }

            //Otherwise, draw the lines between
            int j = GetNextIndex(i);
            Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
        }
#endif
    }
}