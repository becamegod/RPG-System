using System.Collections.Generic;

using UnityEngine;

public abstract class InfoUI : MonoBehaviour
{
    public abstract void UpdateInfo(object subject);
}

public class ItemInfoUI : InfoUI
{
    private IEnumerable<InfoUI> infoUIs;

    private void Awake() => infoUIs = transform.GetComponentsInChildrenOnly<InfoUI>();

    public override void UpdateInfo(object subject)
    {
        foreach (var infoUI in infoUIs) infoUI.UpdateInfo(subject);
    }
}