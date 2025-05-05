using System;
using System.Collections.Generic;

namespace SkillSystem
{
    public interface IPostDamageProcessor
    {
        public List<EffectResult> OnDamageTaken(DamageContext context);
    }

    public class DamageContext
    {
        public readonly BattleSubject attacker;
        public readonly BattleSubject target;
        public readonly int damage;

        public DamageContext(BattleSubject attacker, BattleSubject target, int damage)
        {
            this.attacker = attacker;
            this.target = target;
            this.damage = damage;
        }

        public DamageContext(UsageContext context, int damage)
        {
            attacker = context.user;
            target = context.target;
            this.damage = damage;
        }
    }

    public class CounterProcessor : IPostDamageProcessor
    {
        public SkillDefinition skill;

        BattleSubject owner;
        BattleSubject attacker;

        public List<EffectResult> OnDamageTaken(DamageContext context)
        {
            return context.target.UseSkill(skill, context.attacker);
        }
    }

    public class Recover : IPostDamageProcessor
    {
        public List<EffectResult> OnDamageTaken(DamageContext context)
        {
            throw new NotImplementedException();
        }
    }
}
