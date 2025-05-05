using System;
using UnityEngine;

namespace UISystem
{
    public class ChoicePopup : Singleton<ChoicePopup>
    {
        [SerializeField] GenericText messageText;
        [SerializeField] Menu menu;
        [SerializeField] int acceptIndex = 1;

        private Action onAccept;
        private Action onCancel;

        private new void Awake()
        {
            base.Awake();
            menu.OnSelected += OnSelect;
        }

        private void OnSelect()
        {
            if (menu.OptionIndex == acceptIndex) onAccept?.Invoke();
            else onCancel?.Invoke();
            onAccept = onCancel = null;
        }

        public void Show(string message, Action onAccept = null, Action onCancel = null)
        {
            messageText.Text = message;
            UIController.Instance.Open(menu);
            this.onAccept = onAccept;
            this.onCancel = onCancel;
        }
    }
}
