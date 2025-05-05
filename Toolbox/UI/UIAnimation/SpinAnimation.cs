using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class SpinAnimation : UIAnimation
{
    [SerializeField] Vector3 angleOffset = new(0, 0, -30);
    private Vector3 originalAngle;
    private TweenerCore<Quaternion, Vector3, QuaternionOptions> tween;

    private new void Awake()
    {
        originalAngle = transform.eulerAngles;
        base.Awake();
    }

    public override void Hide()
    {
        base.Hide();
        tween?.Kill();
        tween = transform.DORotate(originalAngle - angleOffset, introDuration)
            .From(originalAngle)
            .SetEase(introEase)
            .OnComplete(() => onShown?.Invoke());
    }

    public override void Show()
    {
        base.Show();
        tween?.Kill();
        tween = transform.DORotate(originalAngle, introDuration)
            .From(originalAngle + angleOffset)
            .SetEase(introEase)
            .OnComplete(() => onShown?.Invoke());
    }

    public override void HideImmediate()
    {
        base.Hide();
        onPreHide?.Invoke();
        tween?.Kill();
        transform.eulerAngles = originalAngle - angleOffset;
        onHidden?.Invoke();
    }

    public override void ShowImmediate()
    {
        base.Show();
        onPreShow?.Invoke();
        tween?.Kill();
        transform.eulerAngles = originalAngle;
        onShown?.Invoke();
    }

    //private void OnDestroy() => tween?.Kill();
}
