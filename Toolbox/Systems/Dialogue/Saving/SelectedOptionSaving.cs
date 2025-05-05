using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "SelectedOptionSaving", menuName = "Dialogue/SelectedOptionSaving")]
    public class SelectedOptionSaving : ScriptableObject
    {
        [ReadOnly, SerializeField] internal int optionIndex = -1;
        public int OptionIndex => optionIndex;
    }
}
