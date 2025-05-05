using System;
using System.Linq;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueBranching : MonoBehaviour
    {
        [Serializable]
        struct DialogueCase
        {
            [SerializeField] int optionIndex;
            [SerializeField] Dialogue[] dialogues;

            public int OptionIndex => optionIndex;
            public Dialogue[] Dialogues => dialogues;
        }

        [SerializeField] Talker talker;
        [SerializeField] SelectedOptionSaving optionSaving;
        [SerializeField] DialogueCase[] dialogueCases;

        private void Reset() => talker = GetComponent<Talker>();

        private void OnEnable() => talker.OnPreTalk += OnPreTalk;

        private void OnDisable() => talker.OnPreTalk -= OnPreTalk;

        private void OnPreTalk()
        {
            var selectedOptionIndex = optionSaving.OptionIndex;
            try
            {
                var dialogueCase = dialogueCases.First(dialogueCase => dialogueCase.OptionIndex == selectedOptionIndex);
                talker.Dialogues = dialogueCase.Dialogues;
            }
            catch (InvalidOperationException)
            {
                if (selectedOptionIndex == -1) return;
                Debug.LogWarning($"The selected option didn't match any of the branching cases: {selectedOptionIndex}");
            }
        }
    }
}
