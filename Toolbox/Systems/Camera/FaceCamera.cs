using UnityEngine;

namespace CameraSystem
{
    public class FaceCamera : MonoBehaviour
    {
        [SerializeField] Vector3 offset;

        enum Style { Orthographic, Perspective }
        [SerializeField] Style style;

        private Camera cam;

        private void Start() => cam = Camera.main;

        private void Update()
        {
            switch (style)
            {
                case Style.Orthographic:
                    transform.rotation = cam.transform.rotation;
                    break;

                case Style.Perspective:
                    transform.LookAt(cam.transform.position);
                    break;
            }
            transform.Rotate(offset);
        }
    }
}
