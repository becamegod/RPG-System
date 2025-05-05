using System;

namespace SkillSystem
{
    // INCOMPLETED

    public interface ISimpleEffect
    {
        EffectResult OnFirstApply();
        EffectResult OnEveryInterval();
        EffectResult OnEffectEnd();
    }

    [Serializable]
    public abstract class SimpleEffect : AutoLabeled
    {
        public abstract void Init(UsageContext context);
        public virtual EffectResult OnFirstApply() => null;
        public virtual EffectResult OnEveryInterval() => null;
        public virtual EffectResult OnEffectEnd() => null;
    }

    public class SimpleEffectLifecycle : IEffect
    {
        UsageContext context;
        SimpleEffect effect;

        public EffectResult OnEffectEnd(UsageContext context) => effect.OnEffectEnd();
        public EffectResult OnEveryInterval(UsageContext context) => effect.OnEveryInterval();
        public EffectResult OnFirstApply(UsageContext context)
        {
            effect.Init(context);
            return effect.OnFirstApply();
        }
    }

    [EffectId("burn")]
    public class SimpleBurn : SimpleEffect
    {
        ScriptableReference health;
        float hpRate;
        private float duration;
        BattleSubject target;

        //BattleSubject user;
        private float burnDamageRate;

        public override void Init(UsageContext context)
        {
            var targetAndUser = context.Infos.Get<SkillUsingInfo>();
            target = targetAndUser.target;

            var user = targetAndUser.user;
            burnDamageRate = user.GetInfo<BurnInfo>().scale;

            var info = EffectInfoMap.Instance.GetInfo(this) as BurnEffectInfo;
            health = info.Health;
            hpRate = info.HpRate;

        }

        public override EffectResult OnFirstApply()
        {
            return base.OnFirstApply();
            //var duration = context.Infos.Get<DurationInfo>().Duration;
            //var lingeringEffect = new LingeringEffect(this, duration);
            //return lingeringEffect.OnFirstApply(context);
        }

        public override EffectResult OnEveryInterval()
        {
            var damage = target.stats.GetBase(health) * hpRate;
            damage *= burnDamageRate;
            return new DamageResult((int)damage);
        }
    }
}
