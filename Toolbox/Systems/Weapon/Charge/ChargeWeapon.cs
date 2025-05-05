using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AssetMenu + "Charge Weapon")]
    public class ChargeWeapon : Weapon
    {
        [HideInInspector] public int possibleStage;
        [SerializeField] private float chargeSpeed = 1f;

        private int stage;
        private float rate;
        private AttackContext context;

        public sealed override void OnPressed(AttackContext context)
        {
            base.OnPressed(context);

            // animation
            var animator = context.WeaponAnimator;
            animator.SetBool("Hold", true);
            animator.speed = 0;

            // rate
            rate = 0;
            this.context = context;
        }

        public override void OnHold()
        {
            base.OnHold();

            // animation
            var animator = context.WeaponAnimator;
            animator.Play("On Hold", 0, rate);

            // rate
            rate += chargeSpeed * Time.deltaTime;
            rate = Mathf.Min(rate, 1);
        }

        public override void OnReleased(AttackContext context)
        {
            base.OnReleased(context);

            // animation
            var animator = context.WeaponAnimator;
            animator.SetBool("Hold", false);
            animator.speed = 1;

            // rate
            context.rate = rate;
            Attack(context);
        }
    }
}
