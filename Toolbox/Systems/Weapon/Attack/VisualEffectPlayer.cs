using UnityEngine;
using UnityEngine.VFX;

using WeaponSystem;

public class VisualEffectPlayer : MonoBehaviour
{
    internal VisualEffect effect;
    public void Play()
    {
        if (!enabled) return;
        effect?.Play();
    }

    public void Play(VisualEffect effect) => effect.Play();
}

public class SetSlashEffectAction : SubAction
{
    [SerializeField] VisualEffectPlayer control;
    [SerializeField] VisualEffect effect;

    public override void Execute(AttackContext context) => control.effect = effect;
}