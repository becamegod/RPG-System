using System;
using UnityEngine;

namespace UISystem
{
    public class BaseUI : VisibilityParent
    {
        [SerializeField] bool autoDetectAnimation = true;
        [SerializeField, ShowWhen("!autoDetectAnimation")] UIAnimation showAnimation;

        [SerializeField] bool persistInStack = true;
        [SerializeField, ShowWhen("persistInStack")] UIAnimation focusAnimation;

        [SerializeField] bool differentOutroAnimation;
        [SerializeField, ShowWhen("differentOutroAnimation")] UIAnimation hideAnimation;
        [SerializeField, ShowWhen("differentOutroAnimation && persistInStack")] UIAnimation lostFocusAnimation;

        // events
        public event Action OnShow, OnShown, OnHide, OnHidden;
        public event Action OnFocus, OnFocused, OnLoseFocus, OnLostFocus;

        // props
        public UIAnimation ShowAnimation => showAnimation;

        protected void Awake()
        {
            if (autoDetectAnimation && !showAnimation) showAnimation = GetComponent<UIAnimation>();
            if (!persistInStack) focusAnimation = showAnimation;
            if (differentOutroAnimation) hideAnimation ??= GetComponent<UIAnimation>();
            else
            {
                lostFocusAnimation = focusAnimation;
                hideAnimation = showAnimation;
            }

            if (showAnimation)
            {
                showAnimation.onPreShow += () => OnShow?.Invoke();
                showAnimation.onShown += () => OnShown?.Invoke();
            }
            if (hideAnimation)
            {
                hideAnimation.onPreHide += () => OnHide?.Invoke();
                hideAnimation.onHidden += () => OnHidden?.Invoke();
            }
            if (focusAnimation)
            {
                focusAnimation.onPreShow += () => OnFocus?.Invoke();
                focusAnimation.onShown += () => OnFocused?.Invoke();
            }
            if (lostFocusAnimation)
            {
                lostFocusAnimation.onPreHide += () => OnLoseFocus?.Invoke();
                lostFocusAnimation.onHidden += () => OnLostFocus?.Invoke();
            }
        }

        public virtual void LoseFocus()
        {
            OnLoseFocus?.Invoke();
            if (lostFocusAnimation) lostFocusAnimation.Hide();
            else OnLostFocus?.Invoke();
        }

        public virtual void Focus()
        {
            OnFocus?.Invoke();
            if (focusAnimation) focusAnimation.Show();
            else OnFocused?.Invoke();
        }

        public override void Show()
        {
            OnShow?.Invoke();
            if (showAnimation) showAnimation.Show();
            else OnShown?.Invoke();
        }

        public override void Hide()
        {
            OnHide?.Invoke();
            if (hideAnimation) hideAnimation.Hide();
            else OnHidden?.Invoke();
        }
    }
}
