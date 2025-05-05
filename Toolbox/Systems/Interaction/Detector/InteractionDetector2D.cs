using UnityEngine;

namespace InteractionSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractionDetector2D : BaseInteractionDetector
    {
        private void OnTriggerEnter2D(Collider2D other) => StartInteraction(other);
        private void OnTriggerExit2D(Collider2D other) => StopInteraction(other);
    }
}