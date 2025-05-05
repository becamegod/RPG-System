using UnityEngine;
using UnityEngine.UI;

public class IconTextbox : MonoBehaviour
{
    [SerializeField] Image icon;
    private GenericText genericText;

    public string Text
    {
        get => GenericText.Text;
        set => GenericText.Text = value;
    }

    public Sprite Icon
    {
        get => icon.sprite;
        set => icon.sprite = value;
    }

    public GenericText GenericText
    {
        get
        {
            if (!genericText) GenericText = GetComponentInChildren<GenericText>();
            return genericText;
        }
        set => genericText = value;
    }

    private void Awake()
    {
        GenericText = GetComponentInChildren<GenericText>();
    }
}