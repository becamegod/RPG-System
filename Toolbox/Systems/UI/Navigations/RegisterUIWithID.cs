using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(BaseUI))]
    public class RegisterUIWithID : MonoBehaviour
    {
        [SerializeField] string id;

        public string Id => id;

        private void Start() => UIIDManager.Instance.RegisterId(id, GetComponent<BaseUI>());

        private void OnDestroy() => UIIDManager.Instance.DeregisterId(id);

        private void Reset() => id = gameObject.name;
    }
}
