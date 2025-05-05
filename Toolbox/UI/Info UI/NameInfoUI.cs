using UnityEngine;

public class NameInfoUI : InfoUI
{
    [SerializeField] GenericText text;

    private void Reset() => text = GetComponent<GenericText>();

    public override void UpdateInfo(object subject)
    {
        if (subject is not IName name) return;
        text.Text = name.Name;
    }
}

public interface IName
{
    public string Name { get; }
}
