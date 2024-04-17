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
    }
}
