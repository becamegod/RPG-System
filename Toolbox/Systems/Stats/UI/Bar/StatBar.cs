using UnityEngine;

public class StatBar : MonoBehaviour
{
    [SerializeField] FillBar bar;
    [SerializeField] Stat stat;
    [SerializeField] float widthScale = 5;

    private void Reset() => bar = GetComponent<FillBar>();

    private void Start()
    {
        bar.Max = stat.Max;
        stat.OnChanged += UpdateBar;
        UpdateBar();
    }

    private void OnDestroy()
    {
        stat.OnChanged -= UpdateBar;
    }

    private void UpdateBar()
    {
        var rt = bar.transform as RectTransform;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stat.Max * widthScale);
        bar.Value = stat.Value;
        bar.Max = stat.Max;
    }
}
