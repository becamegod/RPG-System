using System;
using System.Collections;
using UnityEngine;

public class CameraSequence : MonoBehaviour
{
    [Serializable]
    struct Step
    {
        [SerializeField] float preDelay;
        [SerializeField] CameraFOVAnimation animation;

        internal CameraFOVAnimation Animation => animation;
        internal float PreDelay => preDelay;
    }

    [SerializeField] Step[] steps;

    public int StepIndex
    {
        get => stepIndex;
        set
        {
            value = Mathf.Clamp(value, 0, steps.Length - 1);
            if (stepIndex == value) return;
            stepIndex = value;
            Play();
        }
    }

    public event Action OnCompleted;

    private int stepIndex;
    private Coroutine playCR;

    public void PlayFromStart()
    {
        StepIndex = 0;
        Play();
    }

    public void Play()
    {
        if (playCR != null) StopCoroutine(playCR);
        playCR = StartCoroutine(PlayCR());
        IEnumerator PlayCR()
        {
            for (var stepIndex = 0; stepIndex < steps.Length; stepIndex++)
            {
                var step = steps[stepIndex];
                yield return new WaitForSeconds(step.PreDelay);
                step.Animation.Play();
                yield return new WaitForSeconds(step.Animation.Duration);
            }
            OnCompleted?.Invoke();
            playCR = null;
        }
    }

    public void Revert()
    {
        for (int i = steps.Length - 1; i >= 0; i--) steps[i].Animation.Revert();
    }
}
