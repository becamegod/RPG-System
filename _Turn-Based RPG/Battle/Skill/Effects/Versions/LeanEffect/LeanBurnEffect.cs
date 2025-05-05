using System;

using UnityEngine;

namespace SkillSystem
{
    public class LeanBurnEffect : LeanEffect
    {
        //public override EffectResult OnFirstApply(UsageContext context)
        //{
        //    var duration = context.GetDuration();
        //    var lingeringEffect = new LingeringEffect(this, duration);
        //    return lingeringEffect.OnFirstApply(context);
        //}

        public override EffectResult OnEveryInterval(UsageContext context)
        {
            var info = EffectInfoMap.Instance.GetInfo(this) as BurnEffectInfo;
            var target = context.target;
            var damage = target.stats.GetBase(info.Health) * info.HpRate;
            var burnDamageRate = context.user.GetInfo<BurnInfo>().scale;
            damage *= burnDamageRate;
            return new DamageResult((int)damage);
        }
    }

    public class BurnEffectInfo : EffectInfo
    {
        [SerializeField] ScriptableReference health;
        public ScriptableReference Health => health;

        [SerializeField] float hpRate;
        public float HpRate => hpRate;
    }

    public class BurnInfo : ModifierInfo { }
    public class BurnDamageRateEffect : ModifyScaleEffect<BurnInfo> { }
    public class BurnChanceEffect : ModifyChanceEffect<BurnInfo> { }
}
