using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraSystem
{
    public class LockMouseControl : MonoBehaviour
    {
        [SerializeField] InputAction inputAction;

        private bool locked;
        public bool Locked
        {
            get => locked;
            set
            {
                locked = value;
                Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !locked;
            }
        }

        private void OnEnable()
        {
            inputAction.Enable();
            inputAction.performed += OnPerformed;
        }

        private void OnDisable()
        {
            inputAction.Disable();
            inputAction.performed -= OnPerformed;
        }

        private void OnPerformed(InputAction.CallbackContext obj) => Locked = !Locked;
    }
}