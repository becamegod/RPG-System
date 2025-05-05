using System;
using System.Collections.Generic;
using System.Linq;

using SkillSystem;

using UnityEngine;

namespace ScriptableEffect
{
    static class Global
    {
        public const string AssetMenu = "Scriptable Effect/";
    }

    [CreateAssetMenu(menuName = Global.AssetMenu + "Effect")]
    public class EffectType : ScriptableObject, IEffect
    {
        [SerializeReference] Info info;
        public Info Info => info;

        public virtual EffectResult OnFirstApply(UsageContext context) => null;
        public virtual EffectResult OnEveryInterval(UsageContext context) => null;
        public virtual EffectResult OnEffectEnd(UsageContext context) => null;
        public virtual bool CheckApplicable(UsageContext context) => true;
        public virtual bool CheckSuccessUsage(UsageContext context) => true;
    }

    [Serializable]
    public class EffectConfig
    {
        public EffectType type;
        public float value;
        public float successRate = 1;
        public int duration;
        public bool applyToUser;
    }

    [Serializable]
    public class Effect
    {
        public EffectType type;
        public int turnLeft;
        internal UsageContext context;
        private int originalDuration;

        public bool AffectEveryTurn => type.GetType().GetMethod("OnEveryTurn")?.DeclaringType == type.GetType();

        public float Duration => originalDuration;

        public virtual EffectResult OnFirstApply(UsageContext context)
        {
            var result = type.OnFirstApply(context);
            //result ??= new LingeringEffectResult(this, context);
            return result;
        }

        public virtual EffectResult OnEveryTurn()
        {
            turnLeft--;
            return type.OnEveryInterval(context);
        }

        public virtual EffectResult OnEffectEnd()
        {
            turnLeft = 0;
            //context.target.ActiveEffects.Remove(this);
            return type.OnEffectEnd(context);
        }

        public Effect(EffectConfig details)
        {
            type = details.type;
            turnLeft = details.duration;
            originalDuration = turnLeft;
        }

        public void RefreshDuration() => turnLeft = originalDuration;

        public void OnEffectEnd(UsageContext context)
        {
            throw new NotImplementedException();
        }

        public EffectResult OnEveryInterval(UsageContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class HitInfo
    {
        public enum Type
        {
            Succeed,
            Blocked,
            Critical,
            Missed,
            Heal,
            Bleed,
            Poison
        }

        public readonly Type type;
        public readonly int deltaHp;
        public readonly float elementRate;

        public HitInfo(int deltaHp) => this.deltaHp = deltaHp;

        public HitInfo(Type type) : this(0) => this.type = type;

        public HitInfo(int deltaHp, Type type) : this(type) => this.deltaHp = deltaHp;

        public HitInfo(int deltaHp, float elementRate, Type type) : this(deltaHp, type) => this.elementRate = elementRate;
    }

    public class UsageResult
    {
        public enum MultiHitType
        {
            None,
            Repeat,
            Multiple
        }

        public readonly List<HitInfo> hits;
        public readonly List<Effect> newEffects;
        public bool Missed => (hits.Count == 0 || hits.All(hit => hit.type == HitInfo.Type.Missed)) && newEffects.Count == 0;
        public MultiHitType multiHitType = MultiHitType.None;
        public readonly string message;

        public UsageResult()
        {
            hits = new();
            newEffects = new();
        }

    }
}
