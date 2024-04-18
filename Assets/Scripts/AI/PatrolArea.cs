using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.AI
{
    [RequireComponent(typeof(BoxCollider))]
    public class PatrolArea : MonoBehaviour
    {
        float areaWidth, areaDepth;
        Vector3 generatedPoint;

        BoxCollider areaCol;

        private void Awake()
        {
            areaCol = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            areaWidth = areaCol.bounds.size.x;
            areaDepth = areaCol.bounds.size.z;
            generatedPoint = transform.position;
        }

        public Vector3 GetGeneratedPoint()
        {
            return generatedPoint;
        }

        public void GenerateRandomPoint()
        {
            float x = Random.Range(-areaWidth / 2, areaWidth / 2);
            float z = Random.Range(-areaDepth / 2, areaDepth / 2);

            Vector3 pointInsideVolume = new Vector3(x, 0, z);
            generatedPoint = pointInsideVolume + transform.position;
        }

#if UNITY_EDITOR
        [Header("Editor Gizmo Colors")]
        [SerializeField] Color selectedColor = new Color(1, 1, 1, 0.75f);
        [SerializeField] Color deselectedColor = new Color(1, 1, 1, 0.25f);
        
        private void OnDrawGizmos()
        {
            DrawZone(deselectedColor);
        }

        private void OnDrawGizmosSelected()
        {
            DrawZone(selectedColor);
        }

        void DrawZone(Color color)
        {
            areaCol = GetComponent<BoxCollider>();
            Matrix4x4 rotMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotMatrix;
            Gizmos.color = color;

            Gizmos.DrawCube(areaCol.center, areaCol.size);
        }
#endif
    }
}
