using System.Collections;
using UnityEngine;

namespace CutsceneSystem
{
    public class AutoPlayCutscene : MonoBehaviour
    {
        [SerializeField] Cutscene cutscene;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            CutsceneController.Instance.Play(cutscene);
        }
    }
}
