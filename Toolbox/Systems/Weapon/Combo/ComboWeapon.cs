using System;

using NaughtyAttributes;

using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AssetMenu + "Combo Weapon")]
    public class ComboWeapon : Weapon
    {
        [SerializeField] ComboTree comboTree;
        [ReadOnly] ComboAttackSO currentAttack;

        public event Action<ScriptableObject> OnAttack;

        public override void Init()
        {
            base.Init();
            currentAttack = null;
        }

        public override sealed void OnPressed(AttackContext context)
        {
            //var animator = context.WeaponAnimator;
            //var attack = Attacks[stage] as ComboAttack;
            //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < attack.ActivationRate) return;

            var input = context.Input;
            var tree = currentAttack ? currentAttack.ComboTree : comboTree;
            currentAttack = tree.GetNextAttack(input);

            OnAttack?.Invoke(currentAttack);
        }

        internal override void ResetWeapon()
        {
            base.ResetWeapon();
            currentAttack = null;
        }
    }
}
