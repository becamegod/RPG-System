using UnityEngine;

namespace SkillSystem
{
    public class CriticalChanceEffect : LingeringEffectType
    {
        [SerializeField] float deltaRate;

        public override EffectResult OnFirstApply(UsageContext context)
        {
            context.user.damageCalculator.criticalChanceRate += deltaRate;
            return base.OnFirstApply(context);
        }

        public override EffectResult OnEffectEnd(UsageContext context)
        {
            context.user.damageCalculator.criticalChanceRate -= deltaRate;
            return base.OnEffectEnd(context);
        }
    }

    public class CriticalDamageEffect : LingeringEffectType
    {
        [SerializeField] float deltaDamageRate;

        public override EffectResult OnFirstApply(UsageContext context)
        {
            context.user.damageCalculator.criticalDamageRate += deltaDamageRate;
            return base.OnFirstApply(context);
        }

        public override EffectResult OnEffectEnd(UsageContext context)
        {
            context.user.damageCalculator.criticalDamageRate -= deltaDamageRate;
            return base.OnEffectEnd(context);
        }
    }

    public class CriticalInfo : ModifierInfo
    {
        // override default scale
        public CriticalInfo() => scale = 2;
    }
    public class LeanCriticalChanceEffect : ModifyChanceEffect<CriticalInfo> { }
    public class LeanCriticalDamageEffect : ModifyScaleEffect<CriticalInfo> { }
}
