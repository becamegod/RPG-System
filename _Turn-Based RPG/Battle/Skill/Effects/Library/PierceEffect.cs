using UnityEngine;

namespace SkillSystem
{
    public class PierceEffect : SkillEffect
    {
        [SerializeField] float rate;

        public override EffectResult OnFirstApply(UsageContext context)
        {
            context.user.damageCalculator.defenseSteps.Add(new PiercingStep(rate));
            return null;
        }
    }
}
