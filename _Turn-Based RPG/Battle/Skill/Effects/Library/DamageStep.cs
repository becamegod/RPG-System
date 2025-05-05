namespace SkillSystem
{
    public abstract class DamageStep
    {
        public abstract float Execute(float damage);
    }

    public class BaseValueStep : DamageStep
    {
        float damage;
        public BaseValueStep(float damage) => this.damage = damage;
        public override float Execute(float _) => damage;
    }

    public class CriticalStep : DamageStep
    {
        float chanceRate;
        float damageRate;
        public CriticalStep(float chanceRate, float damageRate)
        {
            this.chanceRate = chanceRate;
            this.damageRate = damageRate;
        }

        bool critical;
        public bool Critical => critical;

        public override float Execute(float damage)
        {
            if (Helper.Chance(chanceRate))
            {
                damage *= 1 + damageRate;
                critical = true;
            }
            return damage;
        }
    }

    public class ReductionStep : DamageStep
    {
        float defense;
        public ReductionStep(float defense) => this.defense = defense;

        public override float Execute(float damage)
        {
            damage *= 100f / (100f + defense);
            return damage;
        }
    }

    public class PiercingStep : DamageStep
    {
        float rate;
        public PiercingStep(float rate) => this.rate = rate;

        public override float Execute(float defense)
        {
            defense *= 1 - rate;
            return defense;
        }
    }
}
