using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionInput : MonoBehaviour
{
    [SerializeField] InputActionReference input;
    [SerializeField] Transform axis;

    private Vector3 direction;

    // props
    public InputAction Action => input.action;
    public Vector3 Value => direction;

    private void Awake()
    {
        if (!Action.enabled) Action.Enable();
    }

    private void OnDestroy()
    {
        if (Action.enabled) Action.Disable();
    }

    private void Update()
    {
        // input
        var input = Action.ReadValue<Vector2>();
        if (input.magnitude == 0) return;
        if (axis) direction = axis.forward * input.y + axis.right * input.x;
        else direction = input;

        // normalize
        direction = direction.normalized;
    }
}

