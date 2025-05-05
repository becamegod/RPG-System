namespace SkillSystem
{
    public abstract class DamageModifier
    {
        public abstract DamageModifierResult Modify(ref float damage);
    }

    public class FinalDamageModifier : DamageModifier
    {
        private BattleSubject subject;
        public FinalDamageModifier(BattleSubject subject) => this.subject = subject;

        public override DamageModifierResult Modify(ref float damage)
        {
            var info = subject.GetInfo<DamageInfo>();
            damage *= info.scale;
            return null;
        }
    }

    public class CriticalDamageModifier : DamageModifier
    {
        private BattleSubject subject;
        public CriticalDamageModifier(BattleSubject subject) => this.subject = subject;

        public override DamageModifierResult Modify(ref float damage)
        {
            var criticalInfo = subject.GetInfo<CriticalInfo>();
            if (Helper.Chance(criticalInfo.chance))
            {
                damage *= criticalInfo.scale;
                return new CriticalResult();
            }
            return null;
        }
    }
}
