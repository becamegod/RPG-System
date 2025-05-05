using System;

using UnityEngine;

namespace SkillSystem
{
    //public class TurnEffect : LingeringEffect
    //{
    //    public TurnEffect(ILingeringEffect details, UsageContext context) : base(details, context) {}
    //    public override float Time => (int)time;
    //    public bool AffectEveryTurn => type.GetType().GetMethod("OnEveryTurn")?.DeclaringType == type.GetType();

    //    public override EffectResult OnEveryInterval()
    //    {
    //        time--;
    //        return type.OnEveryInterval(context);
    //    }
    //}

    [Serializable]
    public class TurnEffectDefinition : ITempEffect
    {
        [SerializeField] int duration;
        public int Duration => duration;

        [SerializeField] bool applyToUser;
        public bool ApplyToUser => applyToUser;

        [SerializeReference, NewButton(true)] LeanEffect effect;
        public LeanEffect Effect => effect;

        public EffectResult OnFirstApply(UsageContext context) => effect.OnFirstApply(context);

        public EffectResult OnEffectEnd(UsageContext context) => effect.OnEffectEnd(context);
    }
}
