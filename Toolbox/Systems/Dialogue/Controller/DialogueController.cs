using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UISystem;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueController : Singleton<DialogueController>, ISavable
    {
        [SerializeField] bool followTalker;

        [Header("References")]
        [SerializeField] TextBox defaultTextBox;
        [SerializeField] Transform defaultTextBoxPos;
        [SerializeField] Menu choiceMenu;
        [SerializeField] UIInteraction choicePrefab;
        [SerializeField] TextInput textInput;

        // props
        private UIController UI => UIController.Instance;
        public Dialogue LastDialogue => lastDialogue;
        public Dialogue FirstDialogue => firstDialogue;

        // events
        public event Action OnStarted, OnEnded;

        // fields
        private Dictionary<Profile, Transform> talkerMap;
        private Dictionary<TextBox, UIFollowWorldObject> worldTextBoxMap;
        private Dictionary<string, TextInputSaving> textSavingMap;
        private SimplePool<UIInteraction> choicePool;
        private Dialogue lastDialogue;
        private Dialogue firstDialogue;

        private new void Awake()
        {
            base.Awake();
            talkerMap = new();
            worldTextBoxMap = new();
            textSavingMap = Resources.LoadAll<TextInputSaving>("TextInputSavings")
                .ToDictionary(saving => saving.Key, saving => saving);
        }

        private void Start()
        {
            choiceMenu.transform.DestroyChildren();
            choicePool = new SimplePool<UIInteraction>(choicePrefab, choiceMenu.transform);
            textInput.OnSubmitted += CloseUI;
        }

        private void CloseUI() => UI.Close();

        public void StartDialogue(Dialogue dialogue, Talker talker = null)
        {
            if (talker != null) talkerMap[talker.Profile] = talker.transform;
            StartCoroutine(DialogueCR(dialogue));

            IEnumerator DialogueCR(Dialogue dialogue)
            {
                // init
                firstDialogue = dialogue;
                var textBox = dialogue.specificTextBox ?? defaultTextBox;
                SetupTextBox(textBox, dialogue[0]);  // to update position & profile info
                UI.Open(textBox);
                textBox.ClearBody();
                OnStarted?.Invoke();

                yield return new WaitForSeconds(textBox.ShowAnimation.IntroDuration);
                yield return StartCoroutine(SubDialogueCR(dialogue));

                UI.Close();
                OnEnded?.Invoke();

                IEnumerator SubDialogueCR(Dialogue dialogue)
                {
                    // init
                    if (dialogue == null) yield break;
                    var index = 0;
                    var sentences = dialogue.Sentences;
                    lastDialogue = dialogue;

                    while (index <= sentences.Count - 1)
                    {
                        var sentence = sentences[index];
                        SetupTextBox(textBox, sentence);
                        textBox.RunText();

                        if (sentence is MultipleChoicesSentence choicesSentence)
                        {
                            yield return new WaitUntil(() => textBox.HaveShownAllText);
                            var menu = choicesSentence.specificMenu ?? choiceMenu;
                            //textBox.OnSelected += CloseUI;
                            UI.Open(menu);

                            yield return new WaitUntil(() => UI.CurrentUI == textBox || UI.CurrentUI == null);
                            //textBox.OnSelected -= CloseUI;
                            var selectionIndex = menu.OptionIndex;
                            var optionSaving = choicesSentence.OptionSaving;
                            if (optionSaving) optionSaving.optionIndex = selectionIndex;
                            var choice = choicesSentence.Choices[selectionIndex];

                            yield return StartCoroutine(SubDialogueCR(choice.Dialogue));
                        }
                        else if (sentence is TextInputSentence textInputSentence)
                        {
                            yield return new WaitUntil(() => textBox.HaveShownAllText);
                            textInput.Label = textInputSentence.Label;
                            textInput.Clear();
                            UI.Open(textInput);

                            yield return new WaitWhile(() => UI.CurrentUI == textInput);
                            textInputSentence.TextSaving.text = textInput.Text;
                        }
                        else yield return new WaitUntil(() => textBox.IsFinished);

                        index++;
                    }
                }
            }
        }

        void SetupTextBox(TextBox textBox, Sentence sentence)
        {
            var profile = sentence.Profile;
            if (textBox.Avatar) textBox.Avatar = profile.Avatar;
            textBox.Name = profile.Name;
            textBox.Content = FillTemplate(textBox, sentence.Content);
            UpdateTextBoxPosition(textBox, profile);
            UpdateChoiceMenu(sentence);
        }

        private string FillTemplate(TextBox textBox, string text)
        {
            int start, end;
            while ((start = text.IndexOf('{')) != -1 && (end = text.IndexOf('}')) != -1)
            {
                var template = text[start..(end + 1)];
                var key = template[1..^1];
                if (textSavingMap.ContainsKey(key))
                {
                    var textSaving = textSavingMap[key];
                    var styleTag = textSaving.StyleTag;
                    var styleExisted = textBox.StyleSheet.GetStyle(styleTag) != null;
                    var value = styleExisted ? $"<style={styleTag}>{textSaving.Text}</style>" : textSaving.Text;
                    text = text.Replace(template, value);
                }
            }
            return text;
        }

        private void UpdateTextBoxPosition(TextBox textBox, Profile profile)
        {
            if (!followTalker) return;

            // register new text box
            if (!worldTextBoxMap.ContainsKey(textBox))
            {
                worldTextBoxMap[textBox] = textBox.GetComponent<UIFollowWorldObject>();
                textBox.OnHidden += () => worldTextBoxMap[textBox]?.Unfollow();
            }

            var worldTextBox = worldTextBoxMap[textBox];
            if (!talkerMap.ContainsKey(profile))
            {
                worldTextBox?.Unfollow();
                if (textBox == defaultTextBox) textBox.transform.position = defaultTextBoxPos.position;
                textBox.TailVisible = false;
            }
            else
            {
                worldTextBox?.Follow(talkerMap[profile].transform);
                textBox.TailVisible = true;
            }
        }

        private void UpdateChoiceMenu(Sentence sentence)
        {
            if (sentence is not MultipleChoicesSentence choicesSentence) return;
            choicePool.DespawnAll();
            var childCount = choicePool.parent.GetComponentsInCloseChildren<Transform>().Count;
            for (int i = 0; i < choicesSentence.Choices.Count; i++)
            {
                var choice = choicesSentence.Choices[i];
                var button = choicePool.Spawn();
                button.GetComponentInChildren<GenericText>().Text = choice.Content;
                button.transform.SetSiblingIndex(i + childCount);
            }
            choiceMenu.Refresh();
        }

        internal void RegisterTalker(Profile profile, Transform talker)
        {
            if (!profile) return;
            if (talkerMap.ContainsKey(profile) && !profile.MultipleTalker)
            {
                Debug.LogWarning($"Talker with profile '{profile.name}' is already existed");
                return;
            }
            talkerMap[profile] = talker;
        }

        internal void DeregisterTalker(Profile profile)
        {
            if (!profile) return;
            talkerMap.Remove(profile);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.End))
            {
                StopAllCoroutines();
                UI.Close();
                OnEnded?.Invoke();
            }
        }
#endif

        public object Save() => new DialogueSaveData();

        public void Load(string dataString)
        {
            var saveData = JsonUtility.FromJson<DialogueSaveData>(dataString);
            foreach (var entry in saveData.Entries)
                DialogueUtils.AllSelectedOptionSavings[entry.Key].optionIndex = entry.Option;
        }
    }

    public class DialogueSaveData
    {
        [Serializable]
        public struct Entry
        {
            [SerializeField] string key;
            [SerializeField] int option;

            public Entry(string key, int option)
            {
                this.key = key;
                this.option = option;
            }

            public string Key => key;
            public int Option => option;
        }

        [SerializeField] List<Entry> entries;

        public List<Entry> Entries => entries;

        public DialogueSaveData()
        {
            var optionSavings = Resources.LoadAll<SelectedOptionSaving>("SelectedOptionSavings");
            entries = optionSavings.Where(optionSaving => optionSaving.OptionIndex != -1)
                .Select(optionSaving => new Entry(optionSaving.name, optionSaving.optionIndex)).ToList();
        }
    }

    public static class DialogueUtils
    {
        private static Dictionary<string, SelectedOptionSaving> allSelectedOptionSavings;
        public static Dictionary<string, SelectedOptionSaving> AllSelectedOptionSavings => allSelectedOptionSavings ??= Resources.LoadAll<SelectedOptionSaving>("SelectedOptionSavings")
                .ToDictionary(definition => definition.name, definition => definition);
    }
}
