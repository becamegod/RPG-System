using DG.Tweening;

using System;

using UnityEngine;

public abstract class UIAnimation : VisibilityParent
{
    [SerializeField] protected float introDuration = .5f;
    [SerializeField] protected float outroDuration = .5f;
    [SerializeField] protected Ease introEase = Ease.OutQuint;
    [SerializeField] protected Ease outroEase = Ease.OutQuint;
    [SerializeField] bool showAtStart;

    public float IntroDuration => introDuration;
    public float OutroDuration => outroDuration;
    public bool Visible => visible;

    public Action onShown, onHidden, onPreShow, onPreHide;

    private bool visible = true;

    public event Action OnAnimating;

    public virtual void ShowImmediate()
    {
        var temp = introDuration;
        introDuration = 0;
        Show();
        introDuration = temp;
    }

    public virtual void HideImmediate()
    {
        var temp = outroDuration;
        outroDuration = 0;
        Hide();
        outroDuration = temp;
    }

    public override void Show() => visible = true;

    public override void Hide() => visible = false;

    protected void RaiseAnimatingEvent() => OnAnimating?.Invoke();

    protected void Awake()
    {
        if (showAtStart || DOTween.IsTweening(gameObject)) return;
        HideImmediate();
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
