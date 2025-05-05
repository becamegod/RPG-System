namespace SkillSystem
{
    public interface IEffect : ITempEffect, IIntervalEffect
    {
    }

    public interface IDuration
    {
        float Duration { get; }
    }

    public interface ILingeringEffect : IEffect, IDuration
    {
    }

    public interface ITempEffect
    {
        EffectResult OnFirstApply(UsageContext context);
        EffectResult OnEffectEnd(UsageContext context);
    }

    public interface IIntervalEffect
    {
        EffectResult OnEveryInterval(UsageContext context);
    }
}