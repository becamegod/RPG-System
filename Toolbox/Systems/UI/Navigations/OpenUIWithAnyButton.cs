using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(AnyButton))]
    public class OpenUIWithAnyButton : MonoBehaviour
    {
        [SerializeField] BaseUI target;

        private void Awake() => GetComponent<AnyButton>().OnTrigger += () => UIController.Instance.Open(target);
    }
}
