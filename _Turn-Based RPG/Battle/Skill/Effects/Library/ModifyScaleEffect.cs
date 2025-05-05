using System;

using UnityEngine;

namespace SkillSystem
{
    [Serializable]
    public abstract class ModifierInfo : Info
    {
        public float chance;
        public float scale = 1;
    }

    public abstract class BaseModifierEffect : LeanEffect
    {
        [SerializeField, TypeDropdown(typeof(ModifierInfo))] string effect;
        [SerializeField] protected float delta;

        public Type EffectType => Type.GetType(effect);

        protected ModifierInfo GetModifierInfo(BattleSubject subject) => subject.GetInfo(EffectType) as ModifierInfo;
    }

    public class ModifyScaleEffect : BaseModifierEffect
    {
        public override EffectResult OnFirstApply(UsageContext context)
        {
            GetModifierInfo(context.target).scale += delta;
            return base.OnFirstApply(context);
        }

        public override EffectResult OnEffectEnd(UsageContext context)
        {
            GetModifierInfo(context.target).scale -= delta;
            return base.OnEffectEnd(context);
        }
    }

    public class ModifyChanceEffect : BaseModifierEffect
    {
        public override EffectResult OnFirstApply(UsageContext context)
        {
            GetModifierInfo(context.target).chance += delta;
            return base.OnFirstApply(context);
        }

        public override EffectResult OnEffectEnd(UsageContext context)
        {
            GetModifierInfo(context.target).chance -= delta;
            return base.OnEffectEnd(context);
        }
    }
}
