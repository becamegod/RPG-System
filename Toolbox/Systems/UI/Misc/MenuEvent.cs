using UnityEngine;
using UnityEngine.Events;

namespace UISystem
{
    [RequireComponent(typeof(Menu))]
    public class MenuEvent : MonoBehaviour
    {
        [SerializeField] UnityEvent onSelect, onFocus;

        private void Awake()
        {
            var menu = GetComponent<Menu>();
            menu.OnPreSelect += () => onSelect?.Invoke();
            menu.OnFocus += () => onFocus?.Invoke();
        }
    }
}
