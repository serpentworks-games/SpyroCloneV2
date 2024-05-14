using UnityEngine;

namespace ScalePact.InteractionSystem.Handlers
{
    public class SimpleObjectTranslator : SimpleObjectTransformer
    {
        [SerializeField] Rigidbody rigidbodyToMove;
        [SerializeField] Vector3 startPos = -Vector3.forward;
        [SerializeField] Vector3 endPos = Vector3.forward;

        public override void PerformTransform(float position)
        {
            float curvePos = accelCurve.Evaluate(position);
            Vector3 pos = transform.TransformPoint(
                Vector3.Lerp(startPos, endPos, curvePos)
            );

            Vector3 deltaPos = pos - rigidbodyToMove.position;

#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                rigidbodyToMove.transform.position = pos;
            }
#endif

            rigidbodyToMove.MovePosition(pos);
        }
    }
}