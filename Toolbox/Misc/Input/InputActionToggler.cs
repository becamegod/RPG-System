using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionToggler : MonoBehaviour
{
    [SerializeField] InputActionReference input;
    private void OnEnable() => input.action.Enable();
    private void OnDisable() => input.action.Disable();
}
