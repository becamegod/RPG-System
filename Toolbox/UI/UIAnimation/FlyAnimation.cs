using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

public class FlyAnimation : UIAnimation
{
    enum Direction
    {
        Up,
        Left,
        Down,
        Right
    }

    struct CanvasPosInfo
    {
        public Vector2 anchorMin, anchorMax;
        public Vector2 position;

        public CanvasPosInfo(RectTransform rt)
        {
            anchorMin = rt.anchorMin;
            anchorMax = rt.anchorMax;
            position = rt.anchoredPosition;
        }

        public void ApplyTo(RectTransform rt)
        {
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.anchoredPosition = position;
        }
    }

    [SerializeField] Direction direction;
    [SerializeField] bool forwardMotion;
    [SerializeField] float offset;

    private RectTransform rt;
    private Vector3[] corners;
    private CanvasPosInfo fromPos;
    private CanvasPosInfo toPos;
    private CanvasPosInfo showPos;

    private new void Awake()
    {
        rt = GetComponent<RectTransform>();
        corners = new Vector3[4];
        showPos = new CanvasPosInfo(rt);
        Setup();
        base.Awake();
    }

    //private void OnValidate()
    //{
    //    if (!Application.isPlaying || !rt) return;
    //    Setup();
    //}

    private void Setup()
    {
        Canvas.ForceUpdateCanvases();
        fromPos = GetCanvasPosInfo(direction);
        toPos = GetCanvasPosInfo(forwardMotion ? OppositeDirection(direction) : direction);
    }

    private Direction OppositeDirection(Direction direction)
    {
        if (direction == Direction.Right) return Direction.Left;
        if (direction == Direction.Left) return Direction.Right;
        if (direction == Direction.Up) return Direction.Down;
        return Direction.Up;
    }

    private CanvasPosInfo GetCanvasPosInfo(Direction direction)
    {
        showPos.ApplyTo(rt);
        switch (direction)
        {
            case Direction.Right:
                rt.anchorMax = rt.anchorMax.WithX(0);
                rt.anchorMin = rt.anchorMin.WithX(0);
                rt.GetWorldCorners(corners);
                rt.anchoredPosition = rt.anchoredPosition.AddX(-corners.Max(corner => corner.x) - offset);
                break;

            case Direction.Left:
                rt.anchorMax = rt.anchorMax.WithX(1);
                rt.anchorMin = rt.anchorMin.WithX(1);
                rt.GetWorldCorners(corners);
                rt.anchoredPosition = rt.anchoredPosition.AddX(Screen.width - corners.Min(corner => corner.x) + offset);
                break;

            case Direction.Up:
                rt.anchorMax = rt.anchorMax.WithY(0);
                rt.anchorMin = rt.anchorMin.WithY(0);
                rt.GetWorldCorners(corners);
                rt.anchoredPosition = rt.anchoredPosition.AddY(-corners.Max(corner => corner.y) - offset);
                break;

            case Direction.Down:
                rt.anchorMax = rt.anchorMax.WithY(1);
                rt.anchorMin = rt.anchorMin.WithY(1);
                rt.GetWorldCorners(corners);
                rt.anchoredPosition = rt.anchoredPosition.AddY(Screen.height - corners.Min(corner => corner.y) + offset);
                break;
        }
        return new CanvasPosInfo(rt);
    }

    private void Transition(CanvasPosInfo from, CanvasPosInfo to, float duration, Ease ease, Action onComplete, Action onStart)
    {
        onStart?.Invoke();
        rt.DOAnchorMin(to.anchorMin, duration).From(from.anchorMin);
        rt.DOAnchorMax(to.anchorMax, duration).From(from.anchorMax);
        rt.DOAnchorPos(to.position, duration)
            .From(from.position)
            .SetEase(ease)
            .OnComplete(() => onComplete?.Invoke());
    }

    public override void Hide()
    {
        base.Hide();
        Transition(showPos, toPos, outroDuration, outroEase, onHidden, onPreHide);
    }

    public override void Show()
    {
        base.Show();
        Transition(fromPos, showPos, introDuration, introEase, onShown, onPreShow);
    }

    private void OnDestroy() => rt.DOKill();
}
