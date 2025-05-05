using UnityEngine;
using UnityEngine.UI;

public class IconInfoUI : InfoUI
{
    [SerializeField] Image image;

    private void Reset() => image = GetComponent<Image>();

    public override void UpdateInfo(object subject)
    {
        if (subject is not IIcon icon) return;
        image.sprite = icon.Icon;
    }
}

public interface IIcon
{
    public Sprite Icon { get; }
}