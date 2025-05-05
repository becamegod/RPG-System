using DG.Tweening;
using UnityEngine;

namespace UISystem
{
    public class AlphaTransition : GraphicTransition
    {
        [SerializeField, Range(0, 1)] float activeAlpha = 1;
        [SerializeField, Range(0, 1)] float inactiveAlpha = .25f;

        protected override void OnExit()
        {
            base.OnEnter();
            graphic.DOKill();
            graphic.DOFade(inactiveAlpha, exitDuration).SetEase(exitEase);
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            graphic.DOKill();
            graphic.DOFade(activeAlpha, enterDuration).SetEase(enterEase);
        }
    }
}
