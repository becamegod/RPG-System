using UnityEngine;

public class NewButtonAttribute : PropertyAttribute
{
    public readonly bool showIfNull;
    public readonly bool persistent;

    public NewButtonAttribute() { }

    public NewButtonAttribute(bool persistent)
    {
        this.persistent = persistent;
    }
}

public interface INewButton
{
}