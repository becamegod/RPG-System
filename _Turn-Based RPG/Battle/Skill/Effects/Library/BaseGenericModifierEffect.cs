namespace SkillSystem
{
    public abstract class BaseGenericModifierEffect<T> : LeanEffect where T : ModifierInfo, new()
    {
    }

    public abstract class ModifyScaleEffect<T> : BaseGenericModifierEffect<T> where T : ModifierInfo, new()
    {
        public override EffectResult OnFirstApply(UsageContext context)
        {
            context.target.GetInfo<T>().scale += context.GetScale();
            return base.OnFirstApply(context);
        }

        public override EffectResult OnEffectEnd(UsageContext context)
        {
            context.target.GetInfo<T>().scale -= context.GetScale();
            return base.OnEffectEnd(context);
        }
    }

    public abstract class ModifyChanceEffect<T> : BaseGenericModifierEffect<T> where T : ModifierInfo, new()
    {
        public override EffectResult OnFirstApply(UsageContext context)
        {
            context.target.GetInfo<T>().chance += context.GetDelta();
            return base.OnFirstApply(context);
        }

        public override EffectResult OnEffectEnd(UsageContext context)
        {
            context.target.GetInfo<T>().chance -= context.GetDelta();
            return base.OnEffectEnd(context);
        }
    }
}
