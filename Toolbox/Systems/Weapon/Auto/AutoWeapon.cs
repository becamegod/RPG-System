using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AssetMenu + "Auto Weapon")]
    public class AutoWeapon : Weapon
    {
        private AttackContext context;

        public override void OnPressed(AttackContext context)
        {
            base.OnPressed(context);
            this.context = context;
        }

        public override void OnHold()
        {
            base.OnHold();
            if (timer == 0)
            {
                timer = interval;
                Attack(context);
            }
        }
    }
}
