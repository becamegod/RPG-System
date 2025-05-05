using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputCallback : MonoBehaviour
{
    [SerializeField] InputActionReference input;
    [SerializeField] UnityEvent onPerformed, onCanceled;

    InputAction Action => input.action;
    public event Action OnPerformed, OnCanceled;

    private void OnEnable()
    {
        Action.performed += OnPressed;
        Action.canceled += OnReleased;
    }

    private void OnDisable()
    {
        Action.performed -= OnPressed;
        Action.canceled -= OnReleased;
    }

    private void OnPressed(InputAction.CallbackContext context)
    {
        onPerformed?.Invoke();
        OnPerformed?.Invoke();
    }

    private void OnReleased(InputAction.CallbackContext context)
    {
        onCanceled?.Invoke();
        OnCanceled?.Invoke();
    }
}
