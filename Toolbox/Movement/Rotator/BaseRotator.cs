using UnityEngine;

namespace MovementSystem
{
    public abstract class BaseRotator : MonoBehaviour, IDirection
    {
        [SerializeField] protected Transform subject;
        [SerializeField] protected float rotateSpeed = 10;

        public abstract Vector3 Direction { get; }

        private void Reset() => subject = transform;

        private void Update()
        {
            if (Direction == Vector3.zero) return;
            subject.forward = Vector3.Slerp(subject.forward, Direction, Time.deltaTime * rotateSpeed);
        }
    }

    public interface IDirection
    {
        Vector3 Direction { get; }
    }
}