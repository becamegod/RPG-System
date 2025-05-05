using System.Collections;
using UnityEngine;

namespace CutsceneSystem
{
    public class ScreenFadeAction : CutsceneAction
    {
        enum FadeMode
        {
            In,
            Out
        }

        [SerializeField] FadeMode fadeMode;
        [SerializeField] bool waitTransition = true;

        private ScreenFader Fader => ScreenFader.Instance;

        public override IEnumerator Play()
        {
            switch (fadeMode)
            {
                case FadeMode.In: 
                    Fader.FadeIn();
                    if (!waitTransition) break;
                    yield return new WaitForSeconds(Mathf.Max(0, Fader.OutroDuration + delay));   // opposite is correct
                    break;
                case FadeMode.Out: 
                    Fader.FadeOut();
                    if (!waitTransition) break;
                    yield return new WaitForSeconds(Mathf.Max(0, Fader.IntroDuration + delay));
                    break;
            }
        }
    }
}
