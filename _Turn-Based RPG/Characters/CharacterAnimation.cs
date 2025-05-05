using System;

using EasyButtons;

using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip baseMeleeClip;
    [SerializeField] AnimationClip baseRangedClip;

    private RuntimeAnimatorController baseController;

    public event Action OnHit;

    private void Reset() => animator = GetComponentInChildren<Animator>();

    private void Awake() => baseController = animator.runtimeAnimatorController;

    public void Hit() => OnHit?.Invoke();

    [Button]
    public void Animate(AnimationClip clip = null, RangeTypeInfo.RangeType rangeType = RangeTypeInfo.RangeType.Melee)
    {
        if (clip) animator.ReplaceClip(baseMeleeClip, clip);
        else animator.runtimeAnimatorController = baseController;

        var trigger = rangeType switch
        {
            RangeTypeInfo.RangeType.Melee => "Melee Action",
            RangeTypeInfo.RangeType.Ranged => "Ranged Action",
            _ => "Melee Action"
        };
        animator.SetTrigger(trigger);
    }

    public void RangedAction(AnimationClip clip = null)
    {
        if (clip) animator.ReplaceClip(baseRangedClip, clip);
        else animator.runtimeAnimatorController = baseController;

        animator.SetTrigger("Ranged Action");
    }

    [Button]
    public void Hurt() => animator.SetTrigger("Hurt");
}
