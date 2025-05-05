using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField] Color highlightColor, unhighlightColor;

    private Image image;
    const float transitionDuration = .15f;

    public string Label
    {
        set => GetComponentInChildren<Text>().text = value;
    }

    public Button.ButtonClickedEvent OnClick => GetComponentInChildren<Button>().onClick;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public virtual void Dehighlight()
    {
        image.DOColor(unhighlightColor, transitionDuration);
    }

    public virtual void Highlight()
    {
        image.DOColor(highlightColor, transitionDuration);
    }
}
