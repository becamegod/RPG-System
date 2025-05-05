using UnityEngine;

namespace InteractionSystem
{
    public abstract class Interaction : MonoBehaviour
    {
        [SerializeField] protected InteractionTrigger trigger;

        protected void OnEnable()
        {
            if (!trigger) return;
            trigger.interaction = this;
            trigger.OnInteract += OnInteract;
        }

        protected void OnDisable()
        {
            if (!trigger) return;
            trigger.OnInteract -= OnInteract;            
        }

        private void Reset() => trigger = GetComponentInChildren<InteractionTrigger>();

        public void ShowIndicator() => trigger.ShowIndicator();
        public void HideIndicator() => trigger.HideIndicator();

        public abstract void OnInteract();
        public abstract bool IsEnded();
    }
}
