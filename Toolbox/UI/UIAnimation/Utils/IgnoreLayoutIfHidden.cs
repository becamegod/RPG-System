using UnityEngine;
using UnityEngine.UI;

public class IgnoreLayoutIfHidden : MonoBehaviour
{
    [SerializeField] UIAnimation uiAnimation;
    [SerializeField] LayoutElement layoutElement;

    private void Reset()
    {
        uiAnimation = GetComponent<UIAnimation>();
        layoutElement = GetComponent<LayoutElement>();
    }

    private void OnEnable()
    {
        uiAnimation.onPreHide += IgnoreLayout;
        uiAnimation.onPreShow += FollowLayout;
    }

    private void OnDisable()
    {
        uiAnimation.onPreHide -= IgnoreLayout;
        uiAnimation.onPreShow -= FollowLayout;
    }

    private void FollowLayout() => layoutElement.ignoreLayout = false;
    private void IgnoreLayout() => layoutElement.ignoreLayout = true;

    private void Start()
    {
        if (!uiAnimation.Visible) IgnoreLayout();
    }
}
