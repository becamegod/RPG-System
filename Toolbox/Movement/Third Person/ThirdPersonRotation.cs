using UnityEngine;

namespace MovementSystem
{
    public class ThirdPersonRotation : MonoBehaviour
    {
        [SerializeField] Transform orientation;
        [SerializeField] float rotateSpeed = 1;

        private void Update() => transform.forward = Vector3.Slerp(transform.forward, orientation.forward, Time.deltaTime * rotateSpeed);
    }
}
