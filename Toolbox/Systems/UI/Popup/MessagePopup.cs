using System;
using UnityEngine;

namespace UISystem
{
    public class MessagePopup : Singleton<MessagePopup>
    {
        [SerializeField] GenericText messageText;
        [SerializeField] Menu menu;
        [SerializeField] RectTransform panel;

        private Action onClose;
        private Vector2 defaultPanelSize;

        private new void Awake()
        {
            base.Awake();
            menu.OnSelected += InvokeCallback;
            menu.OnExit += InvokeCallback;
        }

        private void Start() => defaultPanelSize = panel.sizeDelta;

        private void InvokeCallback()
        {
            onClose?.Invoke();
            onClose = null;
        }

        public void Show(string message, Action onClose = null, float width = -1)
        {
            var panelSize = (width == -1) ? defaultPanelSize : defaultPanelSize.WithX(width);
            panel.sizeDelta = panelSize;
            messageText.Text = message;
            UIController.Instance.Open(menu);
            this.onClose = onClose;
        }

        public void Show(string title, string body, Action onClose = null, float width = -1)
        {
            Show(messageText.With("title", title).With("body", body), onClose, width);
        }
    }
}
