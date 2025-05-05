using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInput : MonoBehaviour
{
    [SerializeField] InputActionReference input;
    [SerializeField] BaseMovement movement;

    // props
    public InputAction Action => input.action;
    public BaseMovement Movement => movement;

    private void Reset() => movement ??= GetComponentInChildren<BaseMovement>();

    private void OnDisable() => movement.ReceiveInput(Vector2.zero);

    private void Update() => movement.ReceiveInput(Action.ReadValue<Vector2>());
}

