using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.UI
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 1f;

        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public IEnumerator FadeScreenOut(float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, 1))
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeScreenOut()
        {
            yield return StartCoroutine(FadeScreenOut(fadeOutTime));
        }

        public IEnumerator FadeScreenIn(float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, 0))
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeScreenIn()
        {
            yield return StartCoroutine(FadeScreenIn(fadeInTime));
        }

        public IEnumerator FadeWait(float time)
        {
            yield return new WaitForSeconds(time);
        }

        public IEnumerator FadeWait()
        {
            yield return new WaitForSeconds(fadeWaitTime);
        }
    }
}