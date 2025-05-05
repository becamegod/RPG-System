using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarMask : Singleton<ProgressBarMask>
{
    [SerializeField] RectTransform widthRef;
    [SerializeField][Range(0, 1)] float value;
    [SerializeField] bool inverse;
    [SerializeField] float speed = 2;
    [SerializeField] bool interpolate;

    private float width;
    private RectMask2D mask;
    private TextMeshProUGUI text;
    public Action onFull;
    public Action<float> onChange;
    private bool full;
    private float prevValue;
    private float currentValue = 0;
    private Vector3 showPos;
    private RectTransform rt;

    private new void Awake()
    {
        base.Awake();
        width = widthRef.rect.width;
        mask = GetComponentInChildren<RectMask2D>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);

        rt = GetComponent<RectTransform>();
        showPos = rt.anchoredPosition;
        rt.anchoredPosition = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (value != prevValue)
        {
            prevValue = value;
            SetPercentage(value);
        }
    }

    private void Update()
    {
        if (!interpolate) return;
        currentValue = Mathf.Lerp(currentValue, value, speed * Time.deltaTime);

        // note: x-left, y-bottom, z-right, w-top
        var padding = mask.padding;
        if (inverse) padding.z = width * (1 - currentValue);
        else padding.z = width * currentValue;
        mask.padding = padding;
    }

    internal void SetPercentage(float v)
    {
        if (!gameObject.activeSelf) return;
        value = v;

        if (!interpolate)
        {
            // note: x-left, y-bottom, z-right, w-top
            var padding = mask.padding;
            if (inverse) padding.z = width * (1 - v);
            else padding.z = width * v;
            mask.padding = padding;
        }

        if ((Mathf.Approximately(v, 1) || v >= 1) && !full)
        {
            onFull?.Invoke();
            full = true;
            v = 1;
        }

        text?.SetText(Mathf.Floor(v * 100).ToString() + "%");
        onChange?.Invoke(v);
    }

    [ContextMenu("Show")]
    internal void Show()
    {
        gameObject.SetActive(true);
        rt.DOAnchorPos(showPos, 1).SetEase(Ease.OutExpo);
    }

    [ContextMenu("Hide")]
    internal void Hide(float duration = -1)
    {
        if (duration == -1) duration = 1;
        rt.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.OutExpo).onComplete += () => gameObject.SetActive(false);
    }
}
