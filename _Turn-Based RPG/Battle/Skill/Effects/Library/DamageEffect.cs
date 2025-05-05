using System.Collections.Generic;

using UnityEngine;

namespace SkillSystem
{
    public class DamageEffect : SkillEffect
    {
        public ScriptableReference scaleStat;
        public float scaleRate;
        public ScriptableReference counterScaleStat;

        public override EffectResult OnFirstApply(UsageContext context)
        {
            base.OnFirstApply(context);
            var target = context.target;

            var canAttack = true;
            if (canAttack)
            {
                var result = CalculateDamage(context.user, target);
                //target.Health.Value -= result.damage;
                return new DamageResult(result.damage);
            }
            return new FailedResult(FailedResult.Type.Blocked);
        }

        private DamageResult CalculateDamage(BattleSubject user, BattleSubject target)
        {
            var myAtk = user.stats[scaleStat] * scaleRate;
            var otherDef = target.stats[counterScaleStat];
            var calculator = user.damageCalculator;

            // base damage
            var damage = calculator.Calculate(myAtk, otherDef);

            // damage modifier
            var modifierResults = new List<DamageModifierResult>();
            foreach (var damageModifier in user.damageModifiers)
            {
                var result = damageModifier.Modify(ref damage);
                if (result != null) modifierResults.Add(result);
            }

            // variety
            damage *= Random.Range(.95f, 1.05f);

            return new((int)damage, modifierResults);
        }
    }

    public class DamageCalculator
    {
        public readonly List<DamageStep> attackSteps = new();
        public readonly List<DamageStep> defenseSteps = new();
        public readonly List<DamageStep> damageSteps = new();

        public float criticalChanceRate = .1f;
        public float criticalDamageRate = 2;

        public float damageRate;
        public float damageIntakeRate;

        public float Calculate(float baseAttack, float baseDefense)
        {
            // calc attack
            var attack = baseAttack;
            foreach (var step in attackSteps) attack = step.Execute(attack);

            // calc defense
            var defense = baseDefense;
            foreach (var step in defenseSteps) defense = step.Execute(defense);

            // calc damage
            var damage = 0f;
            //var criticalStep = new CriticalStep(criticalChanceRate, criticalDamageRate);
            var steps = new List<DamageStep>()
            {
                new BaseValueStep(attack),
                new ReductionStep(defense),
                //criticalStep
            };
            foreach (var step in steps) damage = step.Execute(damage);

            // critical check
            //critical = criticalStep.Critical;

            return damage;
        }
    }
}
