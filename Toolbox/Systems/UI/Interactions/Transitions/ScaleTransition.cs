using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

using UISystem;

using UnityEngine;

public class ScaleTransition : UITransition
{
    [SerializeField] float activeScale = 1.2f;
    [SerializeField] float inactiveScale = 1;
    [SerializeField] bool showFrom0;

    private TweenerCore<Vector3, Vector3, VectorOptions> tween;

    protected override void OnEnter()
    {
        base.OnEnter();
        transform.DOKill();
        tween = transform.DOScale(activeScale, enterDuration).SetEase(enterEase);
        if (showFrom0) tween.From(0);
    }

    protected override void OnExit()
    {
        base.OnExit();
        transform.DOKill();
        tween = transform.DOScale(inactiveScale, exitDuration).SetEase(exitEase);
    }
}
