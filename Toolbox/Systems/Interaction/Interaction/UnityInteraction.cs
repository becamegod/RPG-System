using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem
{
    public class UnityInteraction : Interaction
    {
        [SerializeField] UnityEvent onInteract;
        public override bool IsEnded() => true;
        public override void OnInteract() => onInteract.Invoke();
    }
}
