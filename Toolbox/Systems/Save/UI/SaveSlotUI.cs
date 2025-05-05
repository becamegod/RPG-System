using UnityEngine;
using UnityEngine.UI;

namespace SaveSystem
{
    public class SaveSlotUI : MonoBehaviour
    {
        [SerializeField] GenericText saveTimeText;
        [SerializeField] GenericText locationText;
        [SerializeField] GenericText playerNameText;
        [SerializeField] GenericText playTimeText;
        [SerializeField] GenericText emptyText;
        [SerializeField] GameObject normalView, emptyView;
        [SerializeField] Button deleteButton;

        public Button.ButtonClickedEvent OnDelete => deleteButton.onClick;

        public int Index { get; internal set; }
    }
}
