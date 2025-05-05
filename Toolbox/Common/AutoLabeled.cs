using UnityEngine;

public abstract class AutoLabeled
{
    [ScriptLink]
    [AutoLabel]
    [SerializeField, ReadOnly] string label;
    public AutoLabeled() => label = GetLabel(this);
    public AutoLabeled(object obj) => label = GetLabel(obj);
    private static string GetLabel(object obj) => obj.GetType().Name.SplitPascalCase();
}

public class AutoLabelAttribute : PropertyAttribute { }