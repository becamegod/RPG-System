using System.Collections.Generic;

namespace SkillSystem
{
    public abstract class EffectResult
    {

    }

    public class FailedResult : EffectResult
    {
        public enum Type
        {
            Blocked,
            Missed,
        }
        public readonly Type type;
        public FailedResult(Type type) => this.type = type;
    }

    public class DamageResult : EffectResult
    {
        public readonly int damage;
        public readonly float elementRate;
        public readonly VisualEffectEvent visualEffect;
        public readonly DamageContext context;
        public readonly List<DamageModifierResult> modifierResults = new();

        //public int Damage => damage;
        //public float ElementRate => elementRate;
        //public bool Critical => critical;

        public DamageResult(int damage) => this.damage = damage;

        public DamageResult(int damage, List<DamageModifierResult> modifierResults)
        {
            this.damage = damage;
            this.modifierResults = modifierResults;
        }

        public DamageResult(int damage, VisualEffectEvent visualEffect) : this(damage) => this.visualEffect = visualEffect;
    }

    public class LingeringEffectResult : EffectResult
    {
        public readonly LingeringEffect effect;
        public LingeringEffectResult(LingeringEffect effect) => this.effect = effect;
    }
}
