using System.Collections.Generic;
using UnityEngine;

public abstract class DisableInEvent : MonoBehaviour
{
    [SerializeField] bool respectPrevState = true;
    [SerializeField] Behaviour[] toBeDisabledList;

    // fields
    private Dictionary<Behaviour, bool> prevStatusMap;
    private bool turnedOff;

    protected void Start()
    {
        prevStatusMap = new();
        ListenEvent();
    }

    protected void OnDestroy() => IgnoreEvent();

    protected abstract void ListenEvent();
    protected abstract void IgnoreEvent();

    protected void TurnOn()
    {
        if (!turnedOff) return;
        turnedOff = false;
        foreach (var behavior in toBeDisabledList) behavior.enabled = !respectPrevState || prevStatusMap[behavior];
    }

    protected void TurnOff()
    {
        turnedOff = true;
        foreach (var behavior in toBeDisabledList)
        {
            if (respectPrevState) prevStatusMap[behavior] = behavior.enabled;
            behavior.enabled = false;
        }
    }
}
