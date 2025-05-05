using System;

namespace SkillSystem
{
    public interface IEffectReaction
    {
        EffectResult React(UsageContext context);
    }

    public abstract class EffectReaction : IEffectReaction
    {
        public abstract EffectResult React(UsageContext context);
    }

    public class Deflect : EffectReaction
    {
        public override EffectResult React(UsageContext context)
        {
            throw new NotImplementedException();
        }
    }
}
