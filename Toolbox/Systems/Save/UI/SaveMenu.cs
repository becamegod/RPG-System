using System;

using UISystem;

using UnityEngine;

namespace SaveSystem
{
    public class SaveMenu : MonoBehaviour
    {
        [SerializeField] Menu menu;

        private UIFollowWorldObject worldMenu;

        public event Action OnSaveMenuClosed;

        private void Start()
        {
            worldMenu = menu.GetComponent<UIFollowWorldObject>();
            menu.OnHide += () => OnSaveMenuClosed?.Invoke();
            menu.OnHidden += () => worldMenu?.Unfollow();
        }

        public void OpenMenu(Transform savePoint)
        {
            worldMenu?.Follow(savePoint);
            UIController.Instance.Open(menu);
        }
    }
}
