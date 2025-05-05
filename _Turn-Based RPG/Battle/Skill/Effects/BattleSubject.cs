using System;
using System.Collections.Generic;

using UnityEngine;

namespace SkillSystem
{
    public class BattleSubject : IHealth
    {
        // stats
        public readonly RStats stats;
        public RStat Health => stats["Health"];
        public readonly DamageCalculator damageCalculator = new();
        public readonly List<DamageModifier> damageModifiers = new();

        // info
        private List<ModifierInfo> infos = new();
        public void SetMofifierInfo(List<ModifierInfo> infos) => this.infos = infos;
        public T GetInfo<T>() where T : ModifierInfo, new()
        {
            var info = infos.Get<T>();
            if (info is null)
            {
                info = new T();
                infos.Add(info);
            }
            return info;
        }
        public object GetInfo(Type type)
        {
            if (infos.Get(type) is not ModifierInfo info)
            {
                info = Activator.CreateInstance(type) as ModifierInfo;
                infos.Add(info);
            }
            return info;
        }

        // effects
        [SerializeField] ObservableList<LingeringEffect> effects = new();
        public ObservableList<LingeringEffect> Effects => effects;

        // events
        List<IPostDamageProcessor> damageProcessors = new();

        public BattleSubject(RStats stats) => this.stats = stats;

        //private bool canMove;
        public List<EffectResult> ProcessTurnStart()
        {
            //canMove = true;
            var results = new List<EffectResult>();
            for (var i = effects.Count - 1; i >= 0; i--)
            {
                var effect = effects[i];

                // trigger effects
                var result = effect.OnEveryInterval();
                results.Add(result);

                // remove expired effects
                if (--effect.Time <= 0)
                {
                    effect.OnEffectEnd();
                    effects.Remove(effect);
                }
            }
            return results;
        }

        public List<EffectResult> ProcessTurnEnd()
        {
            //canMove = true;
            var results = new List<EffectResult>();
            for (var i = effects.Count - 1; i >= 0; i--)
            {
                var effect = effects[i];

                // remove expired effects
                if (effect.Time <= 0)
                {
                    effect.OnEffectEnd();
                    effects.Remove(effect);
                }
            }
            return results;
        }

        public List<EffectResult> UseSkill(SkillDefinition skill, BattleSubject target)
        {
            var results = new List<EffectResult>();
            WithEffectAsset(skill, target, results);
            //WithSimpleEffectAsset(skill, target, results);
            //WithLeanEffect(skill, target, results);
            //WithSkillEffect(skill, target, results);
            return results;

            void WithEffectAsset(SkillDefinition skill, BattleSubject target, List<EffectResult> results)
            {
                foreach (var effectParam in skill.EffectParams)
                {
                    // setup context
                    var applyToUser = effectParam.Parameters.Get<ApplyToUserParam>()?.ApplyToUser ?? false;
                    var context = UsageContextFactory.FromBattleSubject(this, applyToUser ? this : target);
                    context.AddParameters(effectParam.Parameters);

                    // add effect
                    var result = target.AddEffect(effectParam.Asset.Effect, context);
                    target.ReactToResult(ref result);
                    results.Add(result);

                    // effect reaction
                    var reactionResult = target.ReactTo(effectParam.GetType(), context);
                    if (reactionResult != null) results.AddRange(reactionResult);
                }
            }

#pragma warning disable CS8321 // Local function is declared but never used
            void WithSimpleEffectAsset(SkillDefinition skill, BattleSubject target, List<EffectResult> results)
            {
                foreach (var effectParam in skill.EffectParams)
                {
                    // get params
                    var applyToUser = effectParam.Parameters.Get<ApplyToUserParam>()?.ApplyToUser ?? false;
                    var duration = effectParam.Parameters.Get<DurationParam>()?.Duration ?? 0;

                    // apply effect
                    var context = UsageContextFactory.FromBattleSubject(this, applyToUser ? this : target);
                    context.Add(new DurationInfo(duration));
                    var effect = Activator.CreateInstance(effectParam.Asset.SimpleEffect.GetType()) as SimpleEffect;
                    effect.Init(context);
                    results.Add(effect.OnFirstApply());

                    // effect reaction
                    var reactionResult = target.ReactTo(effectParam.GetType(), context);
                    if (reactionResult != null) results.AddRange(reactionResult);
                }
            }

            void WithLeanEffect(SkillDefinition skill, BattleSubject target, List<EffectResult> results)
            {
                foreach (var effectDefinition in skill.TurnEffectDefinitions)
                {
                    // apply effect
                    var context = UsageContextFactory.FromBattleSubject(this, effectDefinition.ApplyToUser ? this : target);
                    context.Add(new DurationInfo(effectDefinition.Duration));
                    results.Add(effectDefinition.Effect.OnFirstApply(context));

                    // effect reaction
                    var reactionResult = target.ReactTo(effectDefinition.GetType(), context);
                    if (reactionResult != null) results.AddRange(reactionResult);
                }
            }

            void WithSkillEffect(SkillDefinition skill, BattleSubject target, List<EffectResult> results)
            {
                foreach (var effect in skill.Effects)
                {
                    // apply effect
                    var context = UsageContextFactory.FromBattleSubject(this, target); // missing handle ApplyToUser
                    results.Add(effect.OnFirstApply(context));

                    // effect reaction
                    var reactionResult = target.ReactTo(effect.GetType(), context);
                    if (reactionResult != null) results.AddRange(reactionResult);
                }
            }
#pragma warning restore CS8321 // Local function is declared but never used
        }

        public EffectResult AddEffect(IEffect effect, UsageContext context)
        {
            // create effect
            var duration = context.GetDuration();
            var lingeringEffect = new LingeringEffect(effect, duration, context);

            // apply effect
            effects.Add(lingeringEffect);
            return lingeringEffect.OnFirstApply();
        }

        #region REACTION
        Dictionary<Type, List<IEffectReaction>> reactionMap = new();
        Dictionary<Type, List<IResultReaction>> resultReactionMap = new();

        public void AddEffectResultReaction<T>(IResultReaction<T> reaction) where T : EffectResult
        {
            var type = typeof(T);
            if (!resultReactionMap.ContainsKey(type)) resultReactionMap[type] = new();
            //resultReactionMap[type].Add(reaction);
        }

        private void ReactToResult(ref EffectResult result)
        {
            var type = result.GetType();
            if (!resultReactionMap.ContainsKey(type)) return;
            var reactions = resultReactionMap[type];
            foreach (var reaction in reactions) reaction.React(ref result);
        }

        public void AddEffectReaction<T>(IEffectReaction reaction) where T : SkillEffect
        {
            var type = typeof(T);
            if (!reactionMap.ContainsKey(type)) reactionMap[type] = new();
            reactionMap[type].Add(reaction);
        }

        private List<EffectResult> ReactTo(Type effect, UsageContext context)
        {
            if (!reactionMap.ContainsKey(effect)) return null;
            var reactions = reactionMap[effect];
            var results = new List<EffectResult>();
            foreach (var reaction in reactions) results.Add(reaction.React(context));
            return results;
        }

        public void TakeDamage(DamageContext context)
        {
            ProcessPreDamage(context);
            Health.Value -= context.damage;
            ProcessPostDamage(context);
        }

        void ProcessPreDamage(DamageContext context)
        {
            foreach (var processor in damageProcessors)
            {
                processor.OnDamageTaken(context);
            }
        }

        void ProcessPostDamage(DamageContext context)
        {
            foreach (var processor in damageProcessors)
            {
                processor.OnDamageTaken(context);
            }
        }
        #endregion
    }

    [Serializable]
    public class BattleSubjectInfo : Info
    {
        [SerializeReference] public List<ModifierInfo> infos;
    }
}
