using System;
using System.Linq;

using UISystem;

using UnityEngine;

public class BaseEnterExitEffect : MonoBehaviour
{
    // events
    public event Action OnEnter, OnExit, OnEntered, OnExited;

    public virtual void Enter()
    {
        OnEnter?.Invoke();
        Invoke(nameof(TriggerEntered), longestEnterDuration);
    }

    public virtual void Exit()
    {
        OnExit?.Invoke();
        Invoke(nameof(TriggerExited), longestExitDuration);
    }

    public void TriggerEntered() => OnEntered?.Invoke();
    public void TriggerExited() => OnExited?.Invoke();

    private float longestEnterDuration;
    private float longestExitDuration;

    protected void Start()
    {
        var interactions = GetComponentsInChildren<UITransition>();
        longestEnterDuration = interactions.Max(interaction => interaction.EnterDuration);
        longestExitDuration = interactions.Max(interaction => interaction.ExitDuration);
    }
}