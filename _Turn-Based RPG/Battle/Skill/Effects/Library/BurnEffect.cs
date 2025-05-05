using System;

using ScriptableEffect;

using UnityEngine;

namespace SkillSystem
{
    public class BurnEffect : LingeringEffectType
    {
        [SerializeField] ScriptableReference health;
        [SerializeField] float hpRate;
        [SerializeField] VisualEffectEvent visualEffect;

        public override EffectResult OnEveryInterval(UsageContext context)
        {
            base.OnEveryInterval(context);
            var target = context.GetTarget();
            var damage = (int)(target.stats.GetBase(health) * hpRate);
            target.Health.Value -= damage;
            return new DamageResult(damage, visualEffect);
        }
    }

    public class AddEffect : LeanEffect
    {
        //[SerializeField] ScriptableEffect.Effect effect;
    }

    //public class BurnChanceEffect : LingeringEffectType
    //{
    //    [SerializeField] float deltaRate;

    //    public override EffectResult OnFirstApply(UsageContext context)
    //    {
    //        context.user.damageCalculator.criticalChanceRate += deltaRate;
    //        return base.OnFirstApply(context);
    //    }

    //    public override void OnEffectEnd(UsageContext context)
    //    {
    //        context.user.damageCalculator.criticalChanceRate -= deltaRate;
    //    }
    //}

    public class EffectChanceEffect : LingeringEffectType
    {
        [SerializeField] float deltaRate;
        [SerializeField] SkillEffect effect;

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

    [Serializable]
    public class ChanceToApplyEffect
    {
        [SerializeField] float chance;
        [SerializeField] EffectType effect;
    }
}
