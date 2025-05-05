using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(UIInteraction))]
    public class OpenUIButton : MonoBehaviour
    {
        [SerializeField] BaseUI target;
        private UIInteraction interaction;

        private void Awake() => interaction = GetComponent<UIInteraction>();
        private void OnEnable() => interaction.OnSelect += OnClick;
        private void OnDisable() => interaction.OnSelect -= OnClick;
        private void OnClick() => UIController.Instance.Open(target);
    }
}
