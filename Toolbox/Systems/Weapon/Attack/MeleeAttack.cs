using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AttackMenu + "Melee Attack")]
    public class MeleeAttack : Attack
    {
        public override void Execute(AttackContext context)
        {
            context.WeaponAnimator.SetTrigger("Attack");
        }
    }
}
