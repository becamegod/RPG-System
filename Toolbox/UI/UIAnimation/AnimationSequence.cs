using System;
using System.Collections;
using UnityEngine;

public class AnimationSequence : MonoBehaviour
{
    [Serializable]
    struct Step
    {
        [SerializeField] float preDelay;
        [SerializeField] float transitionOffset;
        [SerializeField] float postDelay;
        [SerializeField] UIAnimation[] animations;

        internal UIAnimation[] Animations => animations;
        internal float PreDelay => preDelay;
        internal float TransitionOffset => transitionOffset;
        internal float PostDelay => postDelay;
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
            ShowPreviousSteps();
            HideNextSteps();
            Play();
        }
    }

    public event Action OnCompleted;

    private void ShowPreviousSteps()
    {
        for (int i = 0; i < stepIndex; i++)
            foreach (var animation in steps[i].Animations)
                animation.ShowImmediate();
    }

    private void HideNextSteps()
    {
        for (int i = stepIndex; i < steps.Length; i++)
            foreach (var animation in steps[i].Animations)
                animation.HideImmediate();
    }

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
            for (; stepIndex < steps.Length; stepIndex++)
            {
                var step = steps[stepIndex];
                yield return new WaitForSeconds(step.PreDelay);
                foreach (var animation in step.Animations)
                {
                    animation.Show();
                    yield return new WaitForSeconds(animation.IntroDuration + step.TransitionOffset);
                }
                yield return new WaitForSeconds(step.PostDelay);
            }
            OnCompleted?.Invoke();
        }
    }
}
