using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class FillAnimation : UIAnimation
{
    [SerializeField] Image image;
    [SerializeField, Range(0, 1)] public float showRate = 1;
    [SerializeField, Range(0, 1)] public float hideRate = 0;
    [SerializeField] bool showFrom0 = true;

    private TweenerCore<float, float, FloatOptions> tween;

    private void Reset() => image = GetComponent<Image>();

    public override void Hide()
    {
        base.Hide();
        onPreHide?.Invoke();
        tween?.Kill();
        tween = image.DOFillAmount(hideRate, outroDuration)
            .SetEase(outroEase)
            .OnComplete(() => onHidden?.Invoke());
    }

    public override void Show()
    {
        base.Show();
        onPreShow?.Invoke();
        tween?.Kill();
        tween = image.DOFillAmount(showRate, introDuration)
            .SetEase(introEase)
            .OnComplete(() => onShown?.Invoke());
        if (showFrom0) tween.From(0);
    }

    public override void HideImmediate()
    {
        base.Hide();
        onPreHide?.Invoke();
        tween?.Kill();
        image.fillAmount = hideRate;
        onHidden?.Invoke();
    }

    public override void ShowImmediate()
    {
        base.Show();
        onPreShow?.Invoke();
        tween?.Kill();
        image.fillAmount = showRate;
        onShown?.Invoke();
    }

    //private void OnDestroy() => tween?.Kill();
}
