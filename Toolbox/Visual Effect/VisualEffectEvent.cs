using System;

using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectEvent : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;
    public VisualEffect VisualEffect => visualEffect;

    public event Action OnEnded;

    bool hasPlayed;

    private void Reset() => visualEffect = GetComponent<VisualEffect>();

    private void Awake()
    {
        if (visualEffect == null) visualEffect = GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if (visualEffect.aliveParticleCount == 0 && hasPlayed)
        {
            OnEnded?.Invoke();
            hasPlayed = false;
            return;
        }

        if (visualEffect.aliveParticleCount > 0) hasPlayed = true;
    }
}
