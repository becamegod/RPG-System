using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    [RequireComponent(typeof(UIInteraction), typeof(Button))]
    public class CloseUIButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<UIInteraction>().OnSelect += Close;
        }

        public void Close() => UIController.Instance.Close();
    }
}
