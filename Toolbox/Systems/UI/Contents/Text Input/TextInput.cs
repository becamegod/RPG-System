using System;
using TMPro;
using UnityEngine;

namespace UISystem
{
    public class TextInput : BaseUI, IExitBlocking
    {
        [SerializeField] TMP_InputField inputField;
        [SerializeField] TextMeshProUGUI labelText;

        // events
        public event Action OnSubmitted;

        // props
        public string Text => inputField.text;
        public string Label
        {
            get => labelText.text;
            set => labelText.text = value;
        }

        private new void Awake()
        {
            base.Awake();
            inputField.onSubmit.AddListener(text =>
            {
                if (text == "")
                {
                    inputField.ActivateInputField();
                    inputField.Select();
                    return;
                }
                OnSubmitted?.Invoke();
            });
            Label = "";
        }

        public override void Focus()
        {
            base.Focus();
            inputField.interactable = true;
            inputField.ActivateInputField();
            inputField.Select();
        }

        public override void LoseFocus()
        {
            base.LoseFocus();
            inputField.DeactivateInputField(true);
            inputField.ReleaseSelection();
            inputField.interactable = false;
            Label = "";
        }

        public bool CanExit() => false;

        private void Reset() => inputField = GetComponent<TMP_InputField>();

        public void Clear() => inputField.text = "";
    }
}
