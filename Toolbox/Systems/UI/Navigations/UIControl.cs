using UISystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIControl : MonoBehaviour
{
    [SerializeField] float navigationAutoFireDelay = 1;
    [SerializeField] float navigationAutoFireInterval = .1f;

    [SerializeField] InputActionReference navigateButton;
    [SerializeField] InputActionReference navigateAltButton;
    [SerializeField] InputActionReference confirmButton;
    [SerializeField] InputActionReference interactButton;
    [SerializeField] InputActionReference optionButton;
    [SerializeField] InputActionReference cancelButton;
    [SerializeField] InputActionReference anyButton;

    private InputAction Navigate => navigateButton.action;
    private InputAction NavigateAlt => navigateAltButton.action;
    private InputAction Confirm => confirmButton.action;
    private InputAction Interact => interactButton.action;
    private InputAction Option => optionButton.action;
    private InputAction Cancel => cancelButton.action;
    private InputAction Any => anyButton.action;
    public UIController UI => UIController.Instance;

    private void Start()
    {
        UI.OnAllowInput += OnAllowInput;
    }

    private void OnDestroy()
    {
        UI.OnAllowInput -= OnAllowInput;
    }

    private void OnAllowInput()
    {
        if (UI.AllowInput)
        {
            if (UI.CurrentUI is not AnyButton) Any.performed -= OnAny;
            else Any.performed += OnAny;
        }
        enabled = UI.AllowInput;
    }

    private void OnCancelNavigate(InputAction.CallbackContext obj) => CancelInvoke(nameof(AutoFireNavigation));

    private void OnEnable()
    {
        Navigate.Enable();
        NavigateAlt.Enable();
        Confirm.Enable();
        Cancel.Enable();
        Interact.Enable();
        Option.Enable();

        Navigate.performed += OnNavigate;
        Navigate.canceled += OnCancelNavigate;
        NavigateAlt.performed += OnNavigateAlt;
        Confirm.performed += OnConfirm;
        Cancel.performed += OnCancel;
        Interact.performed += OnInteract;
        Option.performed += OnOption;
    }

    private void OnDisable()
    {
        Navigate.Disable();
        NavigateAlt.Disable();
        Confirm.Disable();
        Cancel.Disable();
        Any.Disable();
        Interact.Disable();
        Option.Disable();

        Navigate.performed -= OnNavigate;
        Navigate.canceled -= OnCancelNavigate;
        NavigateAlt.performed -= OnNavigateAlt;
        Confirm.performed -= OnConfirm;
        Cancel.performed -= OnCancel;
        Any.performed -= OnAny;
        Interact.performed -= OnInteract;
        Option.performed -= OnOption;
    }

    private void OnNavigate(InputAction.CallbackContext obj)
    {
        InvokeRepeating(nameof(AutoFireNavigation), navigationAutoFireDelay, navigationAutoFireInterval);
        UI.Navigate(obj.ReadValue<Vector2>());
    }

    private void AutoFireNavigation() => UI.Navigate(Navigate.ReadValue<Vector2>());

    private void OnCancel(InputAction.CallbackContext obj) => UI.Cancel();

    private void OnAny(InputAction.CallbackContext obj)
    {
        if (Cancel.WasPerformedThisFrame()) return;
        UI.Any();
    }

    private void OnConfirm(InputAction.CallbackContext obj) => UI.Select(obj.control.name == "leftButton");
    private void OnOption(InputAction.CallbackContext obj) => UI.Option();
    private void OnNavigateAlt(InputAction.CallbackContext obj) => UI.NavigateAlt((int)obj.ReadValue<float>());
    private void OnInteract(InputAction.CallbackContext obj) => UI.Interact();
}
