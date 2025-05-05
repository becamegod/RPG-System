using UnityEngine;

namespace InputHinting
{
    public class HintUI : MonoBehaviour
    {
        [SerializeField] GenericText description;

        public string Description
        {
            get => description.Text;
            set => description.Text = value;
        }
    }
}
