using System;
using System.Collections.Generic;

using UnityEngine;

namespace SkillSystem
{
    [Serializable]
    public abstract class SkillEffect : AutoLabeled, IEffect
    {
        public virtual EffectResult OnFirstApply(UsageContext context) => null;
        public virtual EffectResult OnEveryInterval(UsageContext context) => null;
        public virtual EffectResult OnEffectEnd(UsageContext context) => null;
        public virtual bool CheckApplicable(UsageContext context) => true;
        public virtual bool CheckSuccessUsage(UsageContext context) => true;
    }

    public abstract class LingeringEffectType : SkillEffect, ILingeringEffect
    {
        [SerializeField] float duration = 3;
        public float Duration => duration;

        public override EffectResult OnFirstApply(UsageContext context) => new LingeringEffectResult(new(this, duration, context));
    }

    public class SkillUsingInfo : Info
    {
        public readonly BattleSubject user;
        public readonly BattleSubject target;

        public SkillUsingInfo(BattleSubject user, BattleSubject target)
        {
            this.user = user;
            this.target = target;
        }
    }

    public class UsageContext
    {
        private List<Info> infos = new();
        public IReadOnlyCollection<Info> Infos => infos;
        public void Add(Info info) => infos.Add(info);

        // temp
        public BattleSubject user => this.GetUser();
        public BattleSubject target => this.GetTarget();
    }

    public static class UsageContextFactory
    {
        public static UsageContext FromBattleSubject(BattleSubject user, BattleSubject target)
        {
            var context = new UsageContext();
            context.Add(new SkillUsingInfo(user, target));
            return context;
        }
    }

    // this should be elsewhere
    public static class UsageContextExtension
    {
        public static void AddParameters(this UsageContext context, IEnumerable<EffectParam> parameters)
        {
            foreach (var parameter in parameters) context.Add(parameter);
        }
        public static BattleSubject GetTarget(this UsageContext context) => context.Infos.Get<SkillUsingInfo>()?.target;
        public static BattleSubject GetUser(this UsageContext context) => context.Infos.Get<SkillUsingInfo>()?.user;
        public static bool GetApplyToUser(this UsageContext context) => context.Infos.Get<ApplyToUserParam>()?.ApplyToUser ?? ApplyToUserParam.DefaultValue;
        public static float GetDuration(this UsageContext context) => context.Infos.Get<DurationParam>()?.Duration ?? DurationParam.DefaultValue;
        public static float GetScale(this UsageContext context) => context.Infos.Get<ScaleParam>()?.Scale ?? ScaleParam.DefaultValue;
        public static float GetDelta(this UsageContext context) => context.Infos.Get<DeltaParam>()?.Delta ?? DeltaParam.DefaultValue;
    }

    interface IHealth
    {
        public RStat Health { get; }
    }

    public class PassiveEffectListInfo : Info
    {
        [SerializeField] List<EffectAndParamList> passives;
        public List<EffectAndParamList> Passives => passives;

        //[SerializeField] List<ChanceToApplyEffect> effectChances;
        //public List<ChanceToApplyEffect> EffectChances => effectChances;
    }
}
