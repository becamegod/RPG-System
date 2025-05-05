using UnityEngine;

namespace MovementSystem
{
    public class RotateToDirectionInput : BaseRotator
    {
        [SerializeField] DirectionInput direction;
        public override Vector3 Direction => direction.Value;
    }
}
