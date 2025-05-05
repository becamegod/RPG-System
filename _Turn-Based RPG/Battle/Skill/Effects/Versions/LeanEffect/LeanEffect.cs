using System;

namespace SkillSystem
{
    [Serializable]
    public abstract class LeanEffect : AutoLabeled, IEffect
    {
        public virtual EffectResult OnFirstApply(UsageContext context) => null;
        public virtual EffectResult OnEveryInterval(UsageContext context) => null;
        public virtual EffectResult OnEffectEnd(UsageContext context) => null;
        public virtual bool CheckApplicable(UsageContext context) => true;
        public virtual bool CheckSuccessUsage(UsageContext context) => true;
    }
}
