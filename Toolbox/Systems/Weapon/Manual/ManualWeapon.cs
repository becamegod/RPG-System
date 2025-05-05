using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AssetMenu + "Manual Weapon")]
    public class ManualWeapon : Weapon
    {
        public sealed override void OnPressed(AttackContext context)
        {
            if (timer == 0)
            {
                timer = interval;
                Attack(context);
            }
        }
    }
}
