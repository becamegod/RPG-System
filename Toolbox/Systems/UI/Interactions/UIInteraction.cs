using System;
using UISystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIInteraction : BaseEnterExitEffect, IPointerEnterHandler, IToggleable
{
    // events
    public event Action OnHover, OnSelect, OnStatusChanged;
    public event Action OnToggledOn;
    public event Action OnToggledOff;

    // properties
    private Button Button => GetComponent<Button>();
    private bool IsOnActiveMenu => ui && Controller.CurrentUI == ui;
    private UIController Controller => UIController.Instance;
    private RectTransform Rt => rt != null ? rt : (rt = GetComponent<RectTransform>());
    public bool Interactable
    {
        get => interactable;
        set
        {
            if (interactable == value) return;
            interactable = value;
            OnStatusChanged?.Invoke();
        }
    }
    public bool Highlighted => highlighted;
    public int IndexInMenu => index;

    // fields
    [HideInInspector] public BaseUI ui;
    internal int index;
    private RectTransform rt;
    private Vector3[] corners = new Vector3[4];
    private bool interactable = true;
    private bool highlighted;

    protected new virtual void Start()
    {
        base.Start();
        Button.onClick.AddListener(OnClick);
        Controller.OnNewUIFocused += OnNewUIFocused;
    }

    protected virtual void OnDestroy() => Controller.OnNewUIFocused -= OnNewUIFocused;

    private void OnNewUIFocused()
    {
        if (!ui) return;
        Button.interactable = IsOnActiveMenu;
    }

    public Vector3 GetLeftSide()
    {
        Rt.GetWorldCorners(corners);
        var pos = corners[0];
        pos.y = (corners[0].y + corners[1].y) / 2;
        return pos;
    }

    public Vector3 GetRightCorner()
    {
        Rt.GetWorldCorners(corners);
        return corners[2];
    }

    public Vector3 GetBottomSide()
    {
        Rt.GetWorldCorners(corners);
        var pos = corners[0];
        pos.x = (corners[0].x + corners[3].x) / 2;
        return pos;
    }

    public override void Enter()
    {
        highlighted = true;
        if (!interactable) return;
        base.Enter();
    }

    public virtual void Hover(Vector3 screenPos)
    {
        if (!interactable) return;
        OnHover?.Invoke();
    }

    public override void Exit()
    {
        highlighted = false;
        if (!interactable) return;
        base.Exit();
    }

    public virtual void Select()
    {
        if (!IsOnActiveMenu) return;
        OnSelect?.Invoke();
    }

    public void Toggle(bool on)
    {
        if (!IsOnActiveMenu) return;
        var ev = on ? OnToggledOn : OnToggledOff;
        ev?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Controller.MouseMoved || !IsOnActiveMenu) return;
        Controller.Navigate(index);
        if (ui is Menu menu) menu.lockScroll = true;
    }

    private void OnClick()
    {
        if (!IsOnActiveMenu || !interactable) return;
        Controller.Select(false);
    }

    private void Reset()
    {
        Button.transition = Selectable.Transition.None;
        Button.navigation = new Navigation() { mode = Navigation.Mode.None };
    }
}
