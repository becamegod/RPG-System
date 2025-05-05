using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    [RequireComponent(typeof(Collider))]
    public class InteractionDetector : BaseInteractionDetector
    {
        private void OnTriggerEnter(Collider other) => StartInteraction(other);
        private void OnTriggerExit(Collider other) => StopInteraction(other);
    }

    public abstract class BaseInteractionDetector : MonoBehaviour
    {
        // const
        protected const string interactionTag = "Interaction";
        [SerializeField] InputActionReference interactButton;
        [SerializeField] string compareTag;

        // props
        private InteractionManager Manager => InteractionManager.Instance;

        // events
        public event Action OnInteract;

        private void OnDisable()
        {
            Manager.HideIndicator();
            interactButton.action.Disable();
            interactButton.action.performed -= TriggerInteraction;
        }

        private void OnEnable()
        {
            Manager?.ShowIndicator();
            interactButton.action.Enable();
            interactButton.action.performed += TriggerInteraction;
        }

        protected void StartInteraction(Component other)
        {
            if (compareTag != "" && !other.CompareTag(compareTag)) return;
            if (!enabled) return;
            if (other.TryGetComponent<InteractionTrigger>(out var interaction))
                Manager.OnEnterTrigger(interaction);
        }

        protected void StopInteraction(Component other)
        {
            if (compareTag != "" && !other.CompareTag(compareTag)) return;
            if (!enabled) return;
            if (other.TryGetComponent<InteractionTrigger>(out var interaction))
                Manager.OnExitTrigger(interaction);
        }

        private void TriggerInteraction(InputAction.CallbackContext obj)
        {
            InteractionManager.Instance.Interact();
            OnInteract?.Invoke();
        }
    }
}