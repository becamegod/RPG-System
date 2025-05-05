using UnityEngine;

namespace CutsceneSystem
{
    [RequireComponent(typeof(Collider))]
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] Cutscene cutscene;

        private CutsceneController Controller => CutsceneController.Instance;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Controller.TriggerTagFilter)) return;
            Controller.Play(cutscene);
            GetComponent<Collider>().enabled = false;
        }
    }
}
