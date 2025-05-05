using UnityEngine;

namespace CutsceneSystem
{
    public abstract class RegisterBehaviorId<T> : MonoBehaviour where T : Behaviour
    {
        [SerializeField] string id;

        private void Reset() => id = gameObject.name;

        private void Start() => GetManager().RegisterId(id, GetComponent<T>());

        private void OnDestroy() => GetManager().DeregisterId(id);

        protected abstract BehaviorIdManager<T> GetManager();
    }
}
