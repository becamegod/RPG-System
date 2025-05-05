using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(UIInteraction))]
    public class OpenUIByIdButton : MonoBehaviour
    {
        [SerializeField] string targetId;

        public string TargetId => targetId;

        private void Awake() => GetComponent<UIInteraction>().OnSelect += OnClick;

        private void OnClick()
        {
            var target = UIIDManager.Instance.GetUIWithId(targetId);
            UIController.Instance.Open(target);
        }
    }
}
