using UnityEngine;
using UnityEngine.Events;

public class UIAnimationEvent : MonoBehaviour
{
    [SerializeField] UIAnimation uiAnimation;
    [SerializeField] UnityEvent onShown, onHidden;

    private void Awake()
    {
        if (!uiAnimation) uiAnimation = GetComponent<UIAnimation>();
        uiAnimation.onShown += () => onShown?.Invoke();
        uiAnimation.onHidden += () => onHidden?.Invoke();
    }
}
