using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ColorAdjustableUI : MonoBehaviour
{
    public float transitionDuration = .5f;
    [SerializeField, Range(-1, 1)] float highBrightness = .5f, lowBrightness = -.5f;

    private Material material;
    private Graphic[] graphics;
    private readonly int Saturation = Shader.PropertyToID("_Saturation");
    private readonly int Constrast = Shader.PropertyToID("_Constrast");
    private readonly int Brightness = Shader.PropertyToID("_Brightness");

    private void Awake()
    {
        material = new Material(Shader.Find("UI/ColorAdjustable"));
        graphics = GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics) graphic.material = material;
    }

    private void SetFloat(float value, int propId, float transitionDuration)
    {
        material.DOFloat(value, propId, transitionDuration);
        foreach (var graphic in graphics)
        {
            var materialForRendering = graphic.materialForRendering;
            if (materialForRendering == material) continue;
            if (!materialForRendering.HasFloat(propId)) continue;
            materialForRendering.DOFloat(value, propId, transitionDuration);
        }
    }

    private void SetSaturation(float value) => SetFloat(value, Saturation, transitionDuration);
    public void SaturateHigh() => SetSaturation(2);
    public void SaturateNormal() => SetSaturation(1);
    public void SaturateLow() => SetSaturation(0);

    private void SetConstrast(float value) => SetFloat(value, Constrast, transitionDuration);
    public void ConstrastHigh() => SetConstrast(2);
    public void ConstrastNormal() => SetConstrast(1);
    public void ConstrastLow() => SetConstrast(0);

    private void SetBrightness(float value) => SetFloat(value, Brightness, transitionDuration);
    public void BrightnessHigh() => SetBrightness(highBrightness);
    public void BrightnessNormal() => SetBrightness(0);
    public void BrightnessLow() => SetBrightness(lowBrightness);
}
