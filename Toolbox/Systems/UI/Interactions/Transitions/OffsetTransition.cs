using DG.Tweening;
using UISystem;
using UnityEngine;

public class OffsetTransition : UITransition
{
    [SerializeField] Vector2 offsetOnEnter = new(50, 0);

    private Vector3 basePosition;

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        basePosition = transform.localPosition;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        transform.DOKill();
        transform.DOLocalMove(basePosition + (Vector3)offsetOnEnter, enterDuration).SetEase(enterEase);
    }

    protected override void OnExit()
    {
        base.OnExit();
        transform.DOKill();
        transform.DOLocalMove(basePosition, exitDuration).SetEase(exitEase);
    }
}
