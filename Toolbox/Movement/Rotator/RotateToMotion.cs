using UnityEngine;

namespace MovementSystem
{
    public class RotateToMotion : BaseRotator
    {
        [SerializeField] BaseMovement movement;

        private Vector3 direction;
        public override Vector3 Direction => direction;

        private void OnEnable() => movement.OnMoved += OnMoved;

        private void OnDisable() => movement.OnMoved -= OnMoved;

        private void OnMoved(Vector3 delta)
        {
            if (delta == Vector3.zero) return;
            direction = delta.normalized;
        }
    }
}
