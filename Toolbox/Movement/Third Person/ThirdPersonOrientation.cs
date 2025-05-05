using UnityEngine;

namespace MovementSystem
{
    public class ThirdPersonOrientation : MonoBehaviour
    {
        private Camera cam;

        private void Start() => cam = Camera.main;

        private void Update() => transform.forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
    }
}
