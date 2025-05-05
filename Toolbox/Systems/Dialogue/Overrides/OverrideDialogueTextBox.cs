using UISystem;
using UnityEngine;

namespace DialogueSystem
{
    public class OverrideDialogueTextBox : MonoBehaviour
    {
        [SerializeField] TextBox textBox;
        [SerializeField] Dialogue dialogue;

        private void Reset() => textBox = GetComponent<TextBox>();

        private void Awake() => dialogue.specificTextBox = textBox;

        private void OnDestroy() => dialogue.specificTextBox = null;
    }
}
