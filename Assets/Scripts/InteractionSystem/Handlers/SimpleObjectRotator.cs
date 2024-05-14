using UnityEngine;

namespace ScalePact.InteractionSystem.Handlers
{
    public class SimpleObjectRotator : SimpleObjectTransformer
    {
        [SerializeField] Rigidbody objectToRotate;
        [SerializeField] Vector3 axisToRotateAround = Vector3.forward;
        [Range(0, 360)][SerializeField] float startPosition = 0;
        [Range(0, 360)][SerializeField] float endPosition = 90;

        public override void PerformTransform(float pos)
        {
            float curvePos = accelCurve.Evaluate(pos);
            Quaternion rot = Quaternion.AngleAxis(Mathf.Lerp(startPosition, endPosition, curvePos), axisToRotateAround);

            #if UNITY_EDITOR
            if(Application.isEditor && !Application.isPlaying)
            {
                objectToRotate.transform.localRotation = rot;
            }
#endif

            objectToRotate.transform.localRotation = rot;
        }
    }
}