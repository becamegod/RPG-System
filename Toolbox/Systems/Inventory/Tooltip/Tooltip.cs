using TMPro;
using UnityEngine;

namespace InventorySystem
{
    [RequireComponent(typeof(UIAnimation))]
    public class Tooltip : Singleton<Tooltip>
    {
        [SerializeField] Vector3 offset;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI body;

        private UIAnimation uiAnimation;

        private new void Awake()
        {
            base.Awake();
            uiAnimation = GetComponent<UIAnimation>();
        }

        internal void Show(string title, string body, Vector3 screenPos = default)
        {
            this.title.text = title;
            this.body.text = body;
            if (screenPos != default) Position(screenPos + offset);
            uiAnimation.Show();
        }

        internal void ShowBodyOnly(string body, Vector3 screenPos = default) => Show("", body, screenPos);

        internal void ShowTitleOnly(string title, Vector3 screenPos = default) => Show(title, "", screenPos);

        internal void Hide() => uiAnimation.HideImmediate();

        internal void Position(Vector3 screenPos) => transform.position = screenPos;
    }
}