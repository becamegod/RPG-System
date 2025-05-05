using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

using System;

using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] RectTransform fill;
    [SerializeField, Range(0, 1)] float startRate;

    [Space]
    [SerializeField] bool interpolate;

    [SerializeField, ShowWhen("!interpolate")] bool showDelta;
    [SerializeField, ShowWhen("!interpolate && showDelta")] RectTransform deltaFill;

    [ShowWhen("interpolate || showDelta"), SerializeField] float interpolateDuration = 1;
    [ShowWhen("interpolate || showDelta")] public Ease interpolateEase;
    [ShowWhen("interpolate || showDelta"), SerializeField] bool interpolateValue;

    [Space]
    [SerializeField] bool displayValue;
    [ShowWhen("displayValue"), SerializeField] GenericText valueText;

    [Space]
    [SerializeField] bool dynamicColor;
    [SerializeField, ShowWhen("dynamicColor")] Color highColor = Color.green;
    [SerializeField, ShowWhen("dynamicColor")] Color lowColor = Color.red;

    // props
    private RectTransform Rt => GetComponent<RectTransform>();
    private float Width => Rt.rect.width;
    public float EaseDuration
    {
        get => interpolateDuration;
        set => interpolateDuration = Mathf.Max(0, value);
    }
    public float Max
    {
        get => max;
        set
        {
            if (max == value) return;
            max = value;
            Rate = rate;
            //Value = value;
        }
    }
    public float Value
    {
        get => Rate * Max;
        set
        {
            value = Mathf.Clamp(value, 0, Max);
            if (this.value == value) return;
            this.value = value;
            Rate = value / Max;
        }
    }
    public float Rate
    {
        get => rate;
        set
        {
            //if (rate == value) return;
            beingInterpolatedValue = Value;
            rate = value;

            // value
            if (interpolateValue)
            {
                valueTween?.Kill();
                valueTween = DOTween.To(
                    () => beingInterpolatedValue,
                    x =>
                    {
                        beingInterpolatedValue = x;
                        UpdateValueText(Mathf.CeilToInt(x));
                    },
                    Value,
                    interpolateDuration)
                    .SetEase(interpolateEase);
            }
            else UpdateValueText(Mathf.CeilToInt(Value));

            // bar
            Canvas.ForceUpdateCanvases();
            var finalSizeDelta = new Vector2(rate * Width, fill.sizeDelta.y);
            if (interpolate)
            {
                InterpolateBar(fill, finalSizeDelta);
                UpdateBar(deltaFill, finalSizeDelta);
            }
            else
            {
                if (showDelta)
                {
                    if (finalSizeDelta.x < fill.sizeDelta.x) InterpolateDelta(finalSizeDelta, fill, deltaFill);
                    else InterpolateDelta(finalSizeDelta, deltaFill, fill);
                }
                else
                {
                    UpdateBar(deltaFill, finalSizeDelta);
                    UpdateBar(fill, finalSizeDelta);
                }
            }
        }
    }

    private void InterpolateBar(RectTransform rt, Vector2 sizeTarget)
    {
        barTween?.Kill((RectTransform)barTween.target == deltaFill);
        barTween = DOTween.To(() => rt.sizeDelta, x => UpdateBar(rt, x), sizeTarget, interpolateDuration)
            .SetEase(interpolateEase);
    }

    private void InterpolateDelta(Vector2 sizeTarget, RectTransform instant, RectTransform tweening)
    {
        UpdateBar(instant, sizeTarget);
        InterpolateBar(tweening, sizeTarget);
    }

    private void UpdateBar(RectTransform rt, Vector2 size)
    {
        rt.sizeDelta = size;
        if (dynamicColor && rt == fill) fillImage.color = Color.Lerp(lowColor, highColor, Rate);
    }

    private void UpdateValueText(float x)
    {
        if (!displayValue) return;
        valueText.Text = baseText.Replace("{value}", x.ToString()).Replace("{max}", Max.ToString());
    }

    public void CompleteTweening()
    {
        if (interpolate) barTween.Complete();
        if (interpolateValue) valueTween.Complete();
    }

    // fields
    private float value;
    private float beingInterpolatedValue;
    private TweenerCore<float, float, FloatOptions> valueTween;
    private TweenerCore<Vector2, Vector2, VectorOptions> barTween;
    private Image fillImage;
    private string baseText;
    private float rate;
    private float max;

    private void Awake()
    {
        fillImage = fill.GetComponent<Image>();

        if (!interpolate && !showDelta) interpolateValue = false;
        if (displayValue) baseText = valueText.Text;
        rate = startRate;
        fill.sizeDelta = new Vector2(startRate * Width, fill.sizeDelta.y);
    }

    private void OnDestroy()
    {
        barTween.Kill();
        valueTween.Kill();
    }

    private void OnValidate() => valueText.gameObject.SetActive(displayValue);
}
