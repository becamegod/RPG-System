namespace SkillSystem
{
    public class DamageInfo : ModifierInfo { }
    public class DamageIntakeInfo : ModifierInfo { }

    public class LeanEmpowerEffect : ModifyChanceEffect<DamageInfo> { }
    public class LeanVulnerableEffect : ModifyChanceEffect<DamageIntakeInfo> { }

    public class LeanFortifyEffect : LeanEffect, IResultReaction<DamageResult>
    {
        public override EffectResult OnFirstApply(UsageContext context)
        {
            context.target.AddEffectResultReaction<DamageResult>(this);
            return base.OnFirstApply(context);
        }

        public void React(ref DamageResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}
