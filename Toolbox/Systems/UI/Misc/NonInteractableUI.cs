using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(UIInteraction))]
    public class NonInteractableUI : MonoBehaviour
    {
        private void Start() => GetComponent<UIInteraction>().Interactable = false;
    }
}
