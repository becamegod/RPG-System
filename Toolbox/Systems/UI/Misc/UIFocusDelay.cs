using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(BaseUI))]
    public class UIFocusDelay : MonoBehaviour
    {
        [SerializeField] float delay = 1;
        internal float Delay => delay;
    }
}
