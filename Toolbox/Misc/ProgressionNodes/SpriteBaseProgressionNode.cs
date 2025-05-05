using UnityEngine;
using UnityEngine.UI;

public class SpriteBaseProgressionNode : MonoBehaviour, IProgressionNode
{
    //[SerializeField] Sprite normal;
    //[SerializeField] Sprite highlighted;
    //[SerializeField] Sprite disabled;
    [SerializeField] Color normalColor;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color disabledColor;

    public string text
    {
        get => label.text;
        set => label.text = value;
    }

    private Image image;
    private Text label;

    private void Awake()
    {
        image = GetComponent<Image>();
        label = GetComponentInChildren<Text>();
    }

    public void Dehighlight()
    {
        //image.sprite = normal;
        image.color = normalColor;
    }

    public void Highlight()
    {
        //image.sprite = highlighted;
        image.color = highlightedColor;
    }

    public void Disable()
    {
        //image.sprite = disabled;
        image.color = disabledColor;
    }
}
