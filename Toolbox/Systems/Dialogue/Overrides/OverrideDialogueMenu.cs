using UISystem;
using UnityEngine;

namespace DialogueSystem
{
    public class OverrideDialogueMenu : MonoBehaviour
    {
        [SerializeField] Menu menu;
        [SerializeField] Dialogue dialogue;
        [SerializeField] int sentenceIndex;

        private MultipleChoicesSentence choicesSentence;

        private void Reset() => menu = GetComponent<Menu>();

        private void Awake()
        {
            if (dialogue[sentenceIndex] is MultipleChoicesSentence choicesSentence)
            {
                this.choicesSentence = choicesSentence;
                choicesSentence.specificMenu = menu;
                return;
            }
            Debug.LogWarning($"Index {sentenceIndex} is not a MultipleChoicesSentence");
        }

        private void OnDestroy()
        {
            if (choicesSentence != null) choicesSentence.specificMenu = null;
        }
    }
}
