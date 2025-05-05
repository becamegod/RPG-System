using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UISystem
{
    public class UIController : Singleton<UIController>
    {
        [SerializeField, ReadOnly] BaseUI[] stackDebug;

        // events
        public event Action OnNewUIFocused, OnUIFlowEntered, OnUIFlowExited;
        public event Action OnSelected, OnExitted, OnNavigated;
        public event Action OnDelayedFocus;
        public event Action OnAllowInput;

        // fields
        private Stack<BaseUI> uiStack;
        private Vector3 prevMousePos;
        private bool allowInput;

        // properties
        public BaseUI CurrentUI => (uiStack.Count > 0) ? uiStack.Peek() : null;
        public bool MouseMoved => Input.mousePosition != prevMousePos;

        public bool AllowInput
        {
            get => allowInput;
            set
            {
                if (value == allowInput) return;
                allowInput = value;
                OnAllowInput?.Invoke();
            }
        }

        private new void Awake()
        {
            base.Awake();
            uiStack = new Stack<BaseUI>();
            SceneChanger.OnScenePreLoad += CloseAll;
        }

        private void OnDestroy() => SceneChanger.OnScenePreLoad -= CloseAll;

        public bool Contains(BaseUI ui) => uiStack.Contains(ui);

        public void Open(BaseUI ui)
        {
            var previousUI = CurrentUI;
            uiStack.Push(ui);
            OnUIChange(previousUI, CurrentUI, false);
        }

        public void Close()
        {
            if (uiStack.Count == 0) return;
            var previousUI = CurrentUI;
            uiStack.Pop();
            OnUIChange(previousUI, CurrentUI, true);
        }

        public void ClosePass(BaseUI ui)
        {
            while (CurrentUI != null)
            {
                if (CurrentUI == ui) { Close(); break; }
                Close();
            }
        }

        public void CloseAll()
        {
            while (CurrentUI != null) Close();
            StopAllCoroutines();
        }

        private void OnUIChange(BaseUI previous, BaseUI next, bool isDeregistering)
        {
            stackDebug = uiStack.ToArray();

            // lose focus on previous UI
            if (previous)
            {
                previous.LoseFocus();
                if (isDeregistering) previous.Hide();
            }

            // check for delay
            if (next && next.TryGetComponent<UIFocusDelay>(out var focusDelay))
            {
                StartCoroutine(FocusCR());
                IEnumerator FocusCR()
                {
                    AllowInput = false;
                    OnDelayedFocus?.Invoke();
                    yield return new WaitForSeconds(focusDelay.Delay);
                    FocusNewUI();
                }
            }
            else FocusNewUI();

            void FocusNewUI()
            {
                // focus on new UI
                if (next)
                {
                    if (!isDeregistering) next.Show();
                    next.Focus();
                }

                // toggle input
                if (uiStack.Count == 0) AllowInput = false;
                else AllowInput = true;

                // events
                if (!previous && next) OnUIFlowEntered?.Invoke();
                if (previous && !next) OnUIFlowExited?.Invoke();
                OnNewUIFocused?.Invoke();
            }
        }

        public void Select(bool isLMB)
        {
            if (CurrentUI is not ISelectable selectable) return;
            var selected = selectable.Select(isLMB);
            if (selected) OnSelected?.Invoke();
        }

        private void HandleNavigation(bool navigated)
        {
            if (navigated) OnNavigated?.Invoke();
            if (CurrentUI is Menu menu) menu.lockScroll = false;
        }

        public void Navigate(Vector2 direction)
        {
            if (CurrentUI is not INavigatable navigatable) return;
            var navigated = navigatable.Navigate(direction);
            HandleNavigation(navigated);
        }

        public void Navigate(int index)
        {
            if (CurrentUI is not INavigatable navigatable) return;
            var navigated = navigatable.Navigate(index);
            HandleNavigation(navigated);
            prevMousePos = Input.mousePosition;
        }

        public void Cancel()
        {
            if (CurrentUI is AnyButton) return;
            if (CurrentUI is IExitBlocking ui && !ui.CanExit()) return;
            if (CurrentUI is IExitListener exitListener) exitListener.OnExit();
            Close();
            OnExitted?.Invoke();
        }

        public void Option()
        {
            if (CurrentUI is not IOptionable ui) return;
            ui.Option();
        }

        public void NavigateAlt(int direction)
        {
            if (CurrentUI is not IAltNavigatable navigatable) return;
            navigatable.AltNavigate(direction);
        }
        public void Interact()
        {
            if (CurrentUI is not IInteractable ui) return;
            ui.Interact();
        }
        public void Any()
        {
            if (CurrentUI is not AnyButton anyButton) return;
            var triggered = anyButton.enabled;
            anyButton.Trigger();
            if (triggered) OnSelected?.Invoke();
        }
    }
}