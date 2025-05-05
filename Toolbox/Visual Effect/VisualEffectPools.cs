using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectPools : BasePools<VisualEffectPools, VisualEffectEvent>
{
    public static void Play(VisualEffectEvent visualEffect, Vector3 position)
    {
        var fx = Instance.Spawn(visualEffect, position);
        fx.VisualEffect.Play();
        fx.OnEnded += OnVFXEnded;

        void OnVFXEnded()
        {
            fx.OnEnded -= OnVFXEnded;
            Instance.Despawn(visualEffect, fx);
        }
    }

    public static void Play(VisualEffect vfx, Vector3 position)
    {
        var visualEffectEvent = vfx.GetComponent<VisualEffectEvent>();
        if (!visualEffectEvent) visualEffectEvent = vfx.gameObject.AddComponent<VisualEffectEvent>();
        Play(visualEffectEvent, position);
    }
}