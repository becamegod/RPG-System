using CutsceneSystem;
using DialogueSystem;
using System.Collections;
using UnityEngine;

namespace GameCutscene
{
    public class DialogueAction : CutsceneAction
    {
        [SerializeField] Dialogue dialogue;

        private DialogueController Controller => DialogueController.Instance;

        public override IEnumerator Play()
        {
            yield return base.Play();
            var done = false;
            Controller.OnEnded += SetDone;
            Controller.StartDialogue(dialogue);
            yield return new WaitUntil(() => done);
            Controller.OnEnded -= SetDone;

            void SetDone() => done = true;
        }
    }
}
