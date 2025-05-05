using System;
using UnityEngine;

namespace InteractionSystem
{
    //[RequireComponent(typeof(Collider))]
    public class InteractionTrigger : MonoBehaviour
    {
        [SerializeField] VisibilityParent uiIndicator;
        [SerializeField] bool clickable;

        // events
        public event Action OnInteract;

        // props
        private InteractionManager Manager => InteractionManager.Instance;

        // fields
        internal Interaction interaction;
        private bool canClick;

        private void OnEnable() => canClick = true;

        private void OnDisable()
        {
            canClick = false;
            if (!gameObject.activeInHierarchy) InteractionManager.Instance.OnExitTrigger(this);
        }

        private void OnMouseUpAsButton()
        {
            if (!canClick || !clickable) return;
            Manager.Interact();
        }

        public void ShowIndicator() => uiIndicator.Show();

        public void HideIndicator() => uiIndicator.Hide();

        public void Interact() => OnInteract?.Invoke();
    }
}
