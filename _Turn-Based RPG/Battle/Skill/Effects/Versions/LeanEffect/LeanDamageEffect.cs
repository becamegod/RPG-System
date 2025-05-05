using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace SkillSystem
{
    public class LeanDamageEffect : LeanEffect
    {
        [SerializeField] ScriptableReference scaleStat;
        [SerializeField] ScriptableReference counterScaleStat;

        public override EffectResult OnFirstApply(UsageContext context)
        {
            base.OnFirstApply(context);
            var target = context.target;

            var canAttack = true;
            if (canAttack)
            {
                return CalculateDamage(context.user, target, context.GetScale());
            }
            return new FailedResult(FailedResult.Type.Blocked);
        }

        private DamageResult CalculateDamage(BattleSubject user, BattleSubject target, float scale)
        {
            var myAtk = user.stats[scaleStat] * scale;
            var otherDef = target.stats[counterScaleStat];
            var calculator = user.damageCalculator;

            // final damage
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
}
