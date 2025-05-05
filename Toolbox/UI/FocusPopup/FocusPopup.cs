using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FocusPopup : UIAnimation
{
    [SerializeField] UIAnimation panelAnimation;

    private CanvasGroup canvasGroup;

    private new void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.SetActive(false);
        base.Awake();
    }

    public override void Hide()
    {
        base.Hide();
        canvasGroup.Fade(0, outroDuration);
        panelAnimation.Hide();
    }

    public override void Show()
    {
        base.Show(); 
        canvasGroup.Fade(1, introDuration);
        panelAnimation.Show();
    }
}
