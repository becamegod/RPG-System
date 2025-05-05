using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTemplate
{
    // optimizable
    public class Result
    {
        private readonly string text;
        public Result(string s) => text = s;

        public Result With(string template, object value) => text.Replace($"{{{template}}}", value.ToString());
        public static implicit operator Result(string s) => new(s);
        public static implicit operator string(Result r) => r.text;
    }

    private readonly string baseText;
    private readonly Component component;

    public TextTemplate(TMP_Text textMeshPro)
    {
        baseText = textMeshPro.text;
        component = textMeshPro;
    }

    public TextTemplate(Text text)
    {
        baseText = text.text;
        component = text;
    }

    public void Set(string template, string value)
    {
        var realText = baseText.Replace($"{{{template}}}", value);
        if (component is Text text) text.text = realText;
        else if (component is TMP_Text tmp) tmp.text = realText;
    }

    public void Set(string value) => Set("value", value);

    public Result With(string template, string value) => baseText.Replace($"{{{template}}}", value);
}
