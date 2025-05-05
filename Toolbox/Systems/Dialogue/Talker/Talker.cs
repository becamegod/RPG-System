using InteractionSystem;
using System;
using UnityEngine;

namespace DialogueSystem
{
    public class Talker : Interaction
    {
        [SerializeField] Profile profile;
        [SerializeField] Dialogue[] dialogues;

        // events
        public event Action OnPreTalk, OnDoneTalking;

        // props
        private DialogueController Controller => DialogueController.Instance;
        public Dialogue[] Dialogues
        {
            get => dialogues;
            set
            {
                if (dialogues == value) return;
                dialogues = value;
                dialogueIndex = 0;
            }
        }
        public Profile Profile => profile;

        // fields
        private bool doneTalking;
        private int dialogueIndex;
        private bool started;

        private void Start()
        {
            started = true;
            OnEnable();
        }

        private new void OnEnable()
        {
            if (!started) return;
            base.OnEnable();
            //Controller?.RegisterTalker(profile, this);
            Controller.OnEnded += EndInteraction;
        }

        private void EndInteraction()
        {
            doneTalking = true;
            OnDoneTalking?.Invoke();
        }

        private new void OnDisable()
        {
            base.OnDisable();
            //Controller?.DeregisterTalker(profile);
            Controller.OnEnded -= EndInteraction;
        }

        public override void OnInteract()
        {
            OnPreTalk?.Invoke();
            doneTalking = false;
            //Controller?.StartDialogue(dialogues[dialogueIndex], this);
            Controller?.StartDialogue(dialogues[dialogueIndex], this);
            dialogueIndex = Mathf.Min(dialogueIndex + 1, dialogues.Length - 1);
        }

        public override bool IsEnded() => doneTalking;
    }
}
