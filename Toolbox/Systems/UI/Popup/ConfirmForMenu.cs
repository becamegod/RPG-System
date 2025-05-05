using UnityEngine;
using UnityEngine.Events;

namespace UISystem
{
    public class ConfirmForMenu : MonoBehaviour
    {
        [SerializeField] Menu menu;
        [SerializeField] string message;
        [SerializeField] UnityEvent onAccepted;

        private void Awake()
        {
            menu.OnSelected += Confirm;
        }

        private void Confirm()
        {
            ChoicePopup.Instance.Show(message, () => onAccepted.Invoke());
        }

        private void Reset() => menu = GetComponent<Menu>();
    }
}
