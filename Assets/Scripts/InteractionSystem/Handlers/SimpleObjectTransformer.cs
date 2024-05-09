using UnityEngine;

namespace ScalePact.InteractionSystem.Handlers
{
    public abstract class SimpleObjectTransformer : InteractionHandler
    {
        public enum LoopType
        {
            Once, PingPong, Repeat
        }

        [SerializeField] LoopType loopType;
        [SerializeField] float loopDuration = 1;
        [SerializeField] protected AnimationCurve accelCurve;
        [SerializeField] bool activate = false;
        [SerializeField] SendInteraction OnStartInteraction, OnStopInteraction;
        [SerializeField][Range(0, 1)] float previewPosition;

        float time = 0f;
        float pos = 0f;
        float dir = 1f;

        public float GetPreviewPosition()
        {
            return previewPosition;
        }


        public override void PerformInteraction()
        {
            activate = true;

            if (OnStartInteraction != null)
            {
                OnStartInteraction.Send();
            }
        }

        private void FixedUpdate()
        {
            if (activate)
            {
                time += (dir * Time.deltaTime / loopDuration);

                switch (loopType)
                {
                    case LoopType.Once:
                        LoopOnce();
                        break;
                    case LoopType.PingPong:
                        LoopPingPong();
                        break;
                    case LoopType.Repeat:
                        LoopRepeat();
                        break;
                }

                PerformTransform(pos);
            }
        }

        public virtual void PerformTransform(float pos)
        {

        }

        void LoopOnce()
        {
            pos = Mathf.Clamp01(time);

            if (pos >= 1)
            {
                enabled = false;

                OnStopInteraction?.Send();

                dir *= -1;
            }
        }

        void LoopPingPong()
        {
            pos = Mathf.PingPong(time, 1f);
        }

        void LoopRepeat()
        {
            pos = Mathf.Repeat(time, 1f);
        }
    }
}