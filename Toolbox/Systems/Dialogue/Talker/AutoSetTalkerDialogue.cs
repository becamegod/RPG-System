using System;
using System.Linq;
using UnityEngine;

namespace DialogueSystem
{
    public class AutoSetTalkerDialogue : MonoBehaviour
    {
        [SerializeField] Talker talker;
        [SerializeField] Dialogue[] dialogues;

        private void Reset() => talker = GetComponent<Talker>();

        private void OnEnable()
        {
            talker.Dialogues = dialogues;
            talker.enabled = true;
        }
    }
}
