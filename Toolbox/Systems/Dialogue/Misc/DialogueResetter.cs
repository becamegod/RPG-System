using UnityEngine;

namespace DialogueSystem
{
    public class DialogueResetter : MonoBehaviour
    {
        private void OnDestroy()
        {
            var optionSavings = Resources.LoadAll<SelectedOptionSaving>("SelectedOptionSavings");
            foreach (var optionSaving in optionSavings) optionSaving.optionIndex = -1;
        }
    }
}
