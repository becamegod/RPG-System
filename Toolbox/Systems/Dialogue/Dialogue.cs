using EasyButtons;
using System;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeReference] List<Sentence> sentences;

        public Sentence this[int index] => sentences[Mathf.Clamp(index, 0, sentences.Count - 1)];
        public int Length => sentences.Count;
        public List<Sentence> Sentences => sentences;
        internal TextBox specificTextBox;

        [Button]
        public void AddSentence() => EditorHelper.ShowTypesMenuToAdd(sentences, true, newSentence =>
        {
            var previousSentence = Length < 2 ? null : sentences[^2];
            if (previousSentence == null) return;
            newSentence.Profile = previousSentence.Profile;
        });
    }

    [Serializable]
    public class Sentence
    {
        [SerializeField] Profile profile;
        [SerializeField, TextArea(2, 4)] string content;

        internal Profile Profile { get => profile; set => profile = value; }
        internal string Content => content;
    }

    [Serializable]
    public class MultipleChoicesSentence : Sentence
    {
        [SerializeField] List<Choice> choices;
        [SerializeField] SelectedOptionSaving optionSaving;

        // props
        public List<Choice> Choices => choices;
        public SelectedOptionSaving OptionSaving => optionSaving;

        // fields
        internal Menu specificMenu;

        [Serializable]
        public class Choice
        {
            [SerializeField] string content;
            [SerializeField, Expandable] Dialogue dialogue;
            //[SerializeReference] List<Sentence> sentences;

            public Dialogue Dialogue { get => dialogue; set => dialogue = value; }

            internal string Content => content;
            internal List<Sentence> Sentences => dialogue.Sentences;

            public void AddSubSentence() => EditorHelper.ShowTypesMenuToAdd(Sentences, true, newSentence =>
            {
                var previousSentence = Sentences.Count < 2 ? null : Sentences[^2];
                if (previousSentence == null) return;
                newSentence.Profile = previousSentence.Profile;
            });
        }
    }

    [Serializable]
    class TextInputSentence : Sentence
    {
        [SerializeField] TextInputSaving textSaving;
        [SerializeField] string label;
        internal TextInputSaving TextSaving => textSaving;
        internal string Label => label;
    }
}
