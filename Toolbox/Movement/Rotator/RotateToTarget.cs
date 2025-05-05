using UnityEngine;

namespace MovementSystem
{
    public class RotateToTarget : BaseRotator
    {
        public Transform target;
        [SerializeField] Vector3 scale = Vector3.one;

        public override Vector3 Direction
        {
            get
            {
                if (target == null) return Vector3.zero;
                var direction = target.position - subject.position;
                direction.Scale(scale);
                return direction.normalized;
            }
        }
    }
}
