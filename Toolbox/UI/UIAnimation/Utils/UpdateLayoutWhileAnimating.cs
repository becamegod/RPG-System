using UnityEngine;
using UnityEngine.UI;

public class UpdateLayoutWhileAnimating : MonoBehaviour
{
    [SerializeField] UIAnimation[] uiAnimations;
    [SerializeField] LayoutGroup layoutGroup;

    private void Reset()
    {
        uiAnimations = GetComponentsInChildren<UIAnimation>();
        layoutGroup = GetComponent<LayoutGroup>();
    }

    private void OnEnable()
    {
        foreach (var animation in uiAnimations) animation.OnAnimating += UpdateLayout;
    }

    private void OnDisable()
    {
        foreach (var animation in uiAnimations) animation.OnAnimating -= UpdateLayout;
    }

    private void UpdateLayout()
    {
        layoutGroup.SetLayoutHorizontal();
        layoutGroup.SetLayoutVertical();
    }
}
