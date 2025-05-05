using System.Collections;
using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(BaseUI))]
    public class AutoOpenUI : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            UIController.Instance.Open(GetComponent<BaseUI>());
        }
    }
}
