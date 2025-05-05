using UISystem;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueAudio : GeneralAudio
    {
        [SerializeField] AudioClip characterSound;
        [SerializeField] TextBox textBox;

        private void Start()
        {
            textBox.OnCharacterAdded += () => Play(characterSound);
        }
    }
}
