using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AttackMenu + "Projectile Attack")]
    public class ProjectileAttack : Attack
    {
        [SerializeField] Projectile[] projectiles;

        public override void Execute(AttackContext context)
        {
            foreach (var projectile in projectiles)
            {
                var inst = Instantiate(projectile);
                inst.Launch(context.Source.position, context.Destination.bounds.center);
            }
        }
    }
}
