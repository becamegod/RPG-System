using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InteractionSystem
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        // events
        public event Action OnInteractionStarted, OnInteractionEnded;
        public event Action<bool> OnInteractable;

        // props
        public Interaction CurrentInteraction => triggers.Count > 0 ? triggers[^1].interaction : null;

        // fields
        private List<InteractionTrigger> triggers;

        private new void Awake()
        {
            base.Awake();
            triggers = new();
            SceneManager.sceneLoaded += ClearInteractions;
        }

        private void OnDestroy() => SceneManager.sceneLoaded -= ClearInteractions;

        private void ClearInteractions(Scene arg0, LoadSceneMode arg1)
        {
            triggers = new();
            OnInteractable?.Invoke(false);
        }

        internal void ShowIndicator()
        {
            if (CurrentInteraction) CurrentInteraction.ShowIndicator();
        }

        internal void HideIndicator()
        {
            if (CurrentInteraction) CurrentInteraction.HideIndicator();
        }

        internal void OnEnterTrigger(InteractionTrigger trigger)
        {
            HideIndicator();
            triggers.Add(trigger);
            ShowIndicator();
            if (triggers.Count == 1) OnInteractable?.Invoke(true);
        }

        internal void OnExitTrigger(InteractionTrigger trigger)
        {
            HideIndicator();
            triggers.Remove(trigger);
            ShowIndicator();
            if (triggers.Count == 0) OnInteractable?.Invoke(false);
        }

        public void Interact()
        {
            if (!CurrentInteraction) return;
            StartCoroutine(InteractCR(CurrentInteraction));
        }

        private IEnumerator InteractCR(Interaction interaction)
        {
            interaction.OnInteract();
            OnInteractionStarted?.Invoke();
            yield return new WaitUntil(() => interaction.IsEnded());
            OnInteractionEnded?.Invoke();
        }
    }
}
