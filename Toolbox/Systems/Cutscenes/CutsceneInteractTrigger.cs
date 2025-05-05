using InteractionSystem;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CutsceneSystem
{
    public class CutsceneInteractTrigger : Interaction
    {
        [SerializeField] Cutscene cutscene;
        [SerializeField] UnityEvent onInteract;

        // events
        public event Action OnDoneCutscene;

        // props
        private CutsceneController Controller => CutsceneController.Instance;

        // fields
        private bool doneCutscene;

        private void Start() => Controller.OnEnded += EndInteraction;

        private void EndInteraction()
        {
            doneCutscene = true;
            OnDoneCutscene?.Invoke();
        }

        private void OnDestroy() => Controller.OnEnded -= EndInteraction;

        public override void OnInteract()
        {
            onInteract?.Invoke();
            doneCutscene = false;
            Controller?.Play(cutscene);
        }

        public override bool IsEnded() => doneCutscene;
    }
}
