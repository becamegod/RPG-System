using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AssetMenu + "Generic Weapon")]
    public class GenericWeapon : Weapon
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
