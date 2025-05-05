using UnityEngine;

namespace SkillSystem
{
    public class EmpowerEffect : LingeringEffectType
    {
        [SerializeField] float deltaDamageRate;

        public override EffectResult OnFirstApply(UsageContext context)
        {
            context.user.damageCalculator.damageRate += deltaDamageRate;
            return base.OnFirstApply(context);
        }

        public override EffectResult OnEffectEnd(UsageContext context)
        {
            context.user.damageCalculator.damageRate -= deltaDamageRate;
            return base.OnEffectEnd(context);
        }
    }

    public class FortifyEffect : LingeringEffectType
    {
        [SerializeField] float deltaDamageIntakeRate;

        public override EffectResult OnFirstApply(UsageContext context)
        {
            context.user.damageCalculator.damageIntakeRate += deltaDamageIntakeRate;
            return base.OnFirstApply(context);
        }

        public override EffectResult OnEffectEnd(UsageContext context)
        {
            context.user.damageCalculator.damageIntakeRate -= deltaDamageIntakeRate;
            return base.OnEffectEnd(context);
        }
    }

    public class DamageMultiplyStep : DamageStep
    {
        float multiplier;
        public DamageMultiplyStep(float multiplier) => this.multiplier = multiplier;

        public override float Execute(float damage) => damage * multiplier;
    }
}
