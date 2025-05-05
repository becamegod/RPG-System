using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenericText : MonoBehaviour
{
    // fields
    private Text unityText;
    private TMP_Text tmp;
    private Graphic graphic;
    private TextTemplate template;

    // props
    public string Text
    {
        get => tmp ? tmp.text : unityText.text;
        set
        {
            if (tmp) tmp.text = value;
            else unityText.text = value;
        }
    }

    public Color Color
    {
        get => graphic.color;
        set => graphic.color = value;
    }

    private void Awake()
    {
        unityText = GetComponent<Text>();
        tmp = GetComponent<TMP_Text>();
        graphic = GetComponent<Graphic>();

        if (tmp) template = new TextTemplate(tmp);
        else template = new TextTemplate(unityText);
    }

    public void SetText(string param, string value) => template.Set(param, value);
    public void SetText(string value) => template.Set("value", value);
    public TextTemplate.Result With(string param, object value) => template.With(param, value.ToString());
}
