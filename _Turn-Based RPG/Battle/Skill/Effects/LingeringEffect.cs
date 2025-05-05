using System;

using UnityEngine;

namespace SkillSystem
{
    [Serializable]
    public class LingeringEffect : AutoLabeled, ISimpleEffect
    {
        #region EFFECT
        public readonly IEffect type;
        protected UsageContext context;

        public EffectResult OnFirstApply()
        {
            return type.OnFirstApply(context);
            return new LingeringEffectResult(this);
        }
        public virtual EffectResult OnEveryInterval() => type.OnEveryInterval(context);
        public EffectResult OnEffectEnd() => type.OnEffectEnd(context);
        #endregion

        #region DURATION
        protected float time;
        private float originalDuration;
        public virtual float Time
        {
            get => time;
            set => time = Mathf.Clamp(value, 0, originalDuration);
        }

        public void RefreshDuration() => time = originalDuration;
        #endregion

        public LingeringEffect(IEffect type, float duration, UsageContext context) : base(type)
        {
            this.type = type;
            this.context = context;
            time = duration;
            originalDuration = duration;
        }
    }
}
