using UnityEngine;

namespace CutsceneSystem
{
    [RequireComponent(typeof(Animator))]
    public class RegisterAnimatorId : MonoBehaviour
    {
        [SerializeField] string id;

        private void Reset() => id = gameObject.name;

        private void Start() => AnimatorIdManager.Instance.RegisterId(id, GetComponent<Animator>());

        private void OnDestroy() => AnimatorIdManager.Instance.DeregisterId(id);
    }
}
