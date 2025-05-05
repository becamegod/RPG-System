using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

using UnityEngine;

public class ScaleAnimation : UIAnimation
{
    [SerializeField] float showScale = 1;
    [SerializeField] float hideScale = 0;
    [SerializeField] bool showFrom0;
    private TweenerCore<Vector3, Vector3, VectorOptions> tween;

    public override void Hide()
    {
        base.Hide();
        onPreHide?.Invoke();
        tween?.Kill();
        tween = transform.DOScale(hideScale, outroDuration)
            .SetEase(outroEase)
            .OnUpdate(RaiseAnimatingEvent)
            .OnComplete(() => onHidden?.Invoke());
    }

    public override void Show()
    {
        base.Show();
        onPreShow?.Invoke();
        tween?.Kill();
        tween = transform.DOScale(showScale, introDuration)
            .SetEase(introEase)
            .OnUpdate(RaiseAnimatingEvent)
            .OnComplete(() => onShown?.Invoke());
        if (showFrom0) tween.From(0);
    }

    public override void HideImmediate()
    {
        base.Hide();
        onPreHide?.Invoke();
        tween?.Kill();
        transform.localScale = VectorHelper.UniformVector3(hideScale);
        onHidden?.Invoke();
    }

    public override void ShowImmediate()
    {
        base.Show();
        onPreShow?.Invoke();
        tween?.Kill();
        transform.localScale = VectorHelper.UniformVector3(showScale);
        onShown?.Invoke();
    }

    //private void OnDestroy() => tween?.Kill();
}
