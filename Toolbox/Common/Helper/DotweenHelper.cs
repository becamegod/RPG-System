using Unity.Cinemachine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class DotweenHelper
{
    public static void BlendShape(this SkinnedMeshRenderer smr, int index, float endValue, float duration)
    {
        DOTween.To(() => smr.GetBlendShapeWeight(index), x => smr.SetBlendShapeWeight(index, x), endValue, duration);
    }

    public static TweenerCore<float, float, FloatOptions> Fade(this CanvasGroup canvasGroup, float alpha, float duration)
    {
        canvasGroup.DOKill();
        if (Mathf.Approximately(alpha, 1))
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        return canvasGroup.DOFade(alpha, duration).OnComplete(() =>
        {
            if (Mathf.Approximately(alpha, 0)) canvasGroup.SetActive(false);
        });
    }

    public static void Fade(this Image image, Color color, float duration)
    {
        DOTween.To(() => image.color, x => image.color = x, color, duration);
    }

    //public static TweenerCore<float, float, FloatOptions> DOFOV(this CinemachineCamera vcam, float fov, float duration)
    //{
    //    return DOTween.To(() => vcam.Lens.FieldOfView, x => vcam.Lens.FieldOfView = x, fov, duration);
    //}

    public static TweenerCore<float, float, FloatOptions> DOFOV(this CinemachineCamera vcam, float fov, float duration)
    {
        return DOTween.To(() => vcam.Lens.FieldOfView, x => vcam.Lens.FieldOfView = x, fov, duration);
    }

    //public static void Fade(this Image image, float alpha, float duration)
    //{
    //    DOTween.To(() => image.color.a, x => image.color = new Color(image.color.r, image.color.g, image.color.b, x), alpha, duration);
    //}

    public static void SmoothVertical(this ScrollRect scrollRect, float value, float duration = .5f)
    {
        scrollRect.DOVerticalNormalizedPos(value, duration);
    }
}

[Serializable]
public class Easing
{
    [SerializeField] Ease ease;
    [SerializeField] float duration;

    public Ease Ease => ease;
    public float Duration => duration;
}