using StatsSystem;

using UnityEngine;

public class ReferencedStatBar : MonoBehaviour
{
    [SerializeField] FillBar bar;
    [SerializeField] public RStats stats;
    [SerializeField] ScriptableReference statType;
    [SerializeField] float widthScale = 5;

    BaseStat Current => stats[statType.name];
    BaseStat Max => stats.GetBase(statType.name);

    private void Reset() => bar = GetComponentInChildren<FillBar>();

    private void Start()
    {
        bar.Max = Max.Value;
        Current.OnChanged += UpdateBar;
        Max.OnChanged += UpdateBar;
        UpdateBar(0);
    }

    private void OnDestroy()
    {
        Current.OnChanged -= UpdateBar;
        Max.OnChanged -= UpdateBar;
    }

    private void UpdateBar(float _)
    {
        var rt = bar.transform as RectTransform;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Max * widthScale);
        bar.Value = Current;
        bar.Max = Max;
    }
}
