using System;
using System.Linq;

using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AttackMenu + "Charged Attack")]
    public class ChargedAttack : Attack
    {
        [Serializable]
        public struct Level
        {
            [SerializeField] float rate;
            [SerializeField] Attack attack;

            public float Rate => rate;
            public Attack Attack => attack;
        }

        [SerializeField] Projectile projectile;
        [SerializeField] Level[] levels;

        public override void Execute(AttackContext context)
        {
            var level = levels.Last(level => context.rate > level.Rate);
            level.Attack.Execute(context);
        }
    }
}
