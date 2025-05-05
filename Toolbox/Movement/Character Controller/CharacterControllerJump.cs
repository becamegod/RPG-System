using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerJump : MonoBehaviour
{
    [SerializeField] CharacterControllerVelocity velocity;
    [SerializeField] CharacterController controller;
    [SerializeField] InputActionReference input;
    [SerializeField] float force = 1;

    InputAction Action => input.action;

    private void OnEnable() => Action.performed += Jump;

    private void OnDisable() => Action.performed -= Jump;

    private void Jump(InputAction.CallbackContext obj)
    {
        if (!controller.isGrounded) return;
        velocity.value += Vector3.up * force;
        velocity.Controller.Move(Vector3.zero); // to update controller.isGrounded
    }
}