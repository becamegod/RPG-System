using TMPro;
using UnityEngine;

namespace InventorySystem
{
    [RequireComponent(typeof(UIAnimation))]
    public class SimpleTooltip : MonoBehaviour
    {
        [SerializeField] Vector3 offset;
        [SerializeField] TextMeshProUGUI text;

        private UIAnimation uiAnimation;
        private TextTemplate textTemplate;

        private void Awake()
        {
            uiAnimation = GetComponent<UIAnimation>();
            textTemplate = new TextTemplate(text);
        }

        public void Show(string text, Vector3 screenPos = default)
        {
            this.text.text = text;
            this.UpdateLayout();
            if (screenPos != default) Position(screenPos + offset);
            uiAnimation.Show();
        }

        public void Show(string title, string body, Vector3 screenPos = default)
        {
            Show(textTemplate.With("title", title).With("body", body), screenPos);
        }

        public void Hide() => uiAnimation.HideImmediate();

        internal void Position(Vector3 screenPos) => transform.position = screenPos;
    }
}