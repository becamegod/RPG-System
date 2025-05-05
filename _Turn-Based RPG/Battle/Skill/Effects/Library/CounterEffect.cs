using System.Collections.Generic;

namespace SkillSystem
{
    public class CounterInfo : ModifierInfo { }

    public class CounterEffect : SkillEffect, IEffectReaction
    {
        public override EffectResult OnFirstApply(UsageContext context)
        {
            var owner = context.target;
            owner.AddEffectReaction<DamageEffect>(this);
            return base.OnFirstApply(context);
        }

        public EffectResult React(UsageContext context)
        {
            var target = context.target;
            var chance = target.GetInfo<CounterInfo>().chance;
            if (Helper.Chance(chance)) target.UseSkill(null, context.user);
            return null;
        }
    }

    public class LeanCounterEffect : LeanEffect, IEffectReaction
    {
        public override EffectResult OnFirstApply(UsageContext context)
        {
            var owner = context.target;
            owner.AddEffectReaction<DamageEffect>(this);
            return base.OnFirstApply(context);
        }

        public EffectResult React(UsageContext context)
        {
            var target = context.target;
            var counterInfo = target.GetInfo<CounterInfo>();
            var chance = counterInfo.chance;
            var damageRate = counterInfo.scale;
            if (Helper.Chance(chance)) target.UseSkill(null, context.user);
            return null;
        }
    }

    public class CounterChanceEffect : ModifyChanceEffect<CounterInfo> { }

    public class TurnActionResult : EffectResult
    {
        public readonly SkillDefinition skill;
        public readonly IList<BattleSubject> targets;

        public TurnActionResult(SkillDefinition skill, IList<BattleSubject> targets)
        {
            this.skill = skill;
            this.targets = targets;
        }
    }
}
