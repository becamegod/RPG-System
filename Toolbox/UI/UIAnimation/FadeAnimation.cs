using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeAnimation : UIAnimation
{
    [SerializeField, Range(0, 1)] public float showAlpha = 1;
    [SerializeField, Range(0, 1)] public float hideAlpha = 0;

    protected CanvasGroup canvasGroup;
    private TweenerCore<float, float, FloatOptions> tween;

    private new void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        base.Awake();
    }

    public override void Hide()
    {
        base.Hide();
        onPreHide?.Invoke();
        tween?.Kill();
        //tween = canvasGroup.DOFade(hideAlpha, outroDuration).SetEase(outroEase)
        //    .OnComplete(()=> canvasGroup.SetActive(false));
        tween = canvasGroup.Fade(hideAlpha, outroDuration).SetEase(outroEase);
        tween.onComplete += () => onHidden?.Invoke();
    }

    public override void Show()
    {
        base.Show();
        onPreShow?.Invoke();
        tween?.Kill();
        //canvasGroup.SetActive(true);
        //tween = canvasGroup.DOFade(showAlpha, introDuration).SetEase(introEase);
        tween = canvasGroup.Fade(showAlpha, introDuration).SetEase(introEase);
        tween.onComplete += () => onShown?.Invoke();
    }

    public override void HideImmediate()
    {
        base.Hide();
        onPreHide?.Invoke();
        tween?.Kill();
        canvasGroup.SetActive(false);
        canvasGroup.alpha = hideAlpha;
        onHidden?.Invoke();
    }

    public override void ShowImmediate()
    {
        base.Show();
        onPreShow?.Invoke();
        tween?.Kill();
        canvasGroup.SetActive(true);
        canvasGroup.alpha = showAlpha;
        onShown?.Invoke();
    }

    private void OnDestroy() => tween?.Kill();
}
