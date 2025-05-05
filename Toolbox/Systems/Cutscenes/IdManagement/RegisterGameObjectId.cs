using UnityEngine;

namespace CutsceneSystem
{
    public class RegisterGameObjectId : MonoBehaviour
    {
        [SerializeField] string id;
        [SerializeField] GameObject subject;

        private void Reset()
        {
            id = gameObject.name;
            subject = gameObject;
        }

        private void Start() => GameObjectIdManager.Instance.RegisterId(id, subject);

        private void OnDestroy() => GameObjectIdManager.Instance.DeregisterId(id);
    }
}
