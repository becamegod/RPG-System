using System;
using System.Linq;

public class CompoundUIAnimation : VisibilityParent
{
    private UIAnimation[] animations;

    public event Action onShown, onHidden;

    private void Awake()
    {
        animations = GetComponents<UIAnimation>();

        var longestIntroDuration = animations.Max(animation => animation.IntroDuration);
        var longestOutroDuration = animations.Max(animation => animation.OutroDuration);

        var longestIntro = animations.First(animation => animation.IntroDuration == longestIntroDuration);
        var longestOutro = animations.First(animation => animation.OutroDuration == longestOutroDuration);

        longestIntro.onShown += () => onShown?.Invoke();
        longestOutro.onHidden += () => onHidden?.Invoke();
    }

    public override void Show()
    {
        foreach (var animation in animations) animation.Show();
    }

    public override void Hide()
    {
        foreach (var animation in animations) animation.Hide();
    }
}
