using ScalePact.Core;
using ScalePact.Player;
using UnityEngine;

namespace ScalePact.Combat
{
    [System.Serializable]
    public class EnemyTargetScanner
    {
        [SerializeField] float heightOffset = 0.0f;
        [SerializeField] float detectionRadius = 5f;
        [Range(0, 360)][SerializeField] float detectionAngle = 270;
        [SerializeField] float maxHeightDifference = 1.0f;
        [SerializeField] LayerMask viewBlockingLayers;
        [SerializeField] Color editorGizmoColor = new Color(0, 0, 0.7f, 0.4f);

        Health player;

        public float DetectionRadius { get => detectionRadius; }

        public void FindPlayer()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        public Health GetPlayerRef()
        {
            return player;
        }

        public Health Detect(Transform detector, bool useHeightDif = true)
        {
            Vector3 eyePos = detector.position + Vector3.up * heightOffset;
            Vector3 toPlayer = player.transform.position - eyePos;
            Vector3 toPlayerTop = player.transform.position + Vector3.up * 1.5f - eyePos; // HISS

            //If player is too height or low, don't target them
            if (useHeightDif && Mathf.Abs(toPlayer.y + heightOffset) > maxHeightDifference)
            {
                return null;
            }

            Vector3 toPlayerFlat = toPlayer;
            toPlayerFlat.y = 0;

            if (toPlayerFlat.sqrMagnitude <= detectionRadius * detectionRadius)
            {
                if (Vector3.Dot(toPlayerFlat.normalized, detector.forward) >
                    Mathf.Cos(detectionAngle * 0.5f * Mathf.Deg2Rad))
                {
                    bool canSee = false;
#if UNITY_EDITOR
                    Debug.DrawRay(eyePos, toPlayer, Color.blue);
                    Debug.DrawRay(eyePos, toPlayerTop, Color.blue);
#endif
                    canSee |= !Physics.Raycast(
                        eyePos, toPlayer.normalized, detectionRadius,
                        viewBlockingLayers, QueryTriggerInteraction.Ignore
                    );

                    canSee |= !Physics.Raycast(
                        eyePos, toPlayerTop.normalized, detectionRadius,
                        viewBlockingLayers, QueryTriggerInteraction.Ignore
                    );

                    if (canSee) return player;
                }
            }

            return null;
        }

#if UNITY_EDITOR
        public void EditorGizmo(Transform transform)
        {
            UnityEditor.Handles.color = editorGizmoColor;
            Vector3 rotatedForward = Quaternion.Euler(0, -detectionAngle * 0.5f, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(
                transform.position, Vector3.up, rotatedForward, detectionAngle, detectionRadius);

            Gizmos.color = new(1, 1, 0, 1);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * heightOffset, 0.2f);
        }
#endif
    }
}