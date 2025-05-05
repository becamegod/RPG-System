namespace SkillSystem
{
    public interface IResultReaction : IResultReaction<EffectResult>
    {
    }

    public interface IResultReaction<T> where T : EffectResult
    {
        void React(ref T result);
    }
}
