using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "NewProfile", menuName = "Dialogue/Profile")]
    public class Profile : ScriptableObject
    {
        [SerializeField] new string name;
        [SerializeField] Sprite avatar;
        [SerializeField] bool multipleTalker;

        internal string Name => name;
        internal Sprite Avatar => avatar;
        internal bool MultipleTalker => multipleTalker;
    }
}
