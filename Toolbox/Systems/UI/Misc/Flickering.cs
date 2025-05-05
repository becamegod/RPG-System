using System;
using System.Collections;
using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Flickering : MonoBehaviour
    {
        [SerializeField] float duration = .5f;
        [SerializeField] int quickFlickNumber = 3;
        [SerializeField] float quickFlickDuration = .1f;

        private CanvasGroup canvasGroup;
        private float originalAlpha;
        private Coroutine flickeringCR;
        private Action afterQuickFlick;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            originalAlpha = canvasGroup.alpha;
        }

        private void OnEnable()
        {
            flickeringCR = StartCoroutine(FlickeringCR(duration));
        }

        private void OnDisable()
        {
            StopCoroutine(flickeringCR);
            canvasGroup.alpha = originalAlpha;
        }

        public void QuickFlick(Action callback = null)
        {
            afterQuickFlick = callback;
            StopCoroutine(flickeringCR);
            flickeringCR = StartCoroutine(FlickeringCR(quickFlickDuration, quickFlickNumber));
        }

        private IEnumerator FlickeringCR(float duration, int number = -1)
        {
            while (number != 0)
            {
                yield return new WaitForSeconds(duration);
                canvasGroup.alpha = 0;
                yield return new WaitForSeconds(duration);
                canvasGroup.alpha = originalAlpha;
                number--;
            }
            afterQuickFlick?.Invoke();
            afterQuickFlick = null;
        }
    }
}
