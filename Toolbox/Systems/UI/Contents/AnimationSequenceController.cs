using UnityEngine;

namespace UISystem
{
    public class AnimationSequenceController : BaseUI, ISelectable, IExitBlocking
    {
        [SerializeField] AnimationSequence animationSequence;

        public bool CanExit() => false;

        public bool Select(bool isLMB)
        {
            var prevStepIndex = animationSequence.StepIndex;
            animationSequence.StepIndex++;
            return prevStepIndex != animationSequence.StepIndex;
        }

        private void Reset()
        {
            animationSequence = GetComponent<AnimationSequence>();
        }
    }
}
