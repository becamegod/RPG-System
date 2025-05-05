using UnityEngine;

namespace WeaponSystem
{
    [RequireComponent(typeof(GenericMotion))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] Hitbox hitbox;
        [SerializeField] float directionOffset;

        // fields
        private GenericMotion motion;

        // props
        public Hitbox Hitbox => hitbox;

        private void Awake()
        {
            hitbox ??= GetComponentInChildren<Hitbox>();
            //hitbox.OnHit += DestroySelf;
            motion = GetComponent<GenericMotion>();
        }

        private void OnDestroy()
        {
            //hitbox.OnHit -= DestroySelf;
        }

        public void DestroySelf() => Destroy(gameObject);

        public void DestroySelf(float delay) => Destroy(gameObject, delay);

        public void SetSpeed(float value)
        {
            speed = value;
            motion.Move(transform.forward *  speed);
        }

        public void Launch(Vector3 from, Vector3 to)
        {
            // position
            transform.position = from;

            // velocity
            var direction = (to - from).normalized;
            motion.Move(direction * speed);

            // rotation
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void OnBecameInvisible() => DestroySelf();

        public void StuckToTarget(GameObject target, Vector3 pos)
        {
            transform.parent = target.transform;
        }
    }
}
