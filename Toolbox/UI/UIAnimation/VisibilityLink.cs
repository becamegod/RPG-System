using UnityEngine;

[RequireComponent(typeof(UIAnimation))]
public class VisibilityLink : MonoBehaviour
{
    [SerializeField] UIAnimation linkFrom;
    [SerializeField] UIAnimation linkTo;
    [SerializeField] bool oppositePhase;

    private void Reset() => linkFrom = GetComponent<UIAnimation>();

    private void Awake()
    {
        if (!linkFrom) linkFrom = GetComponent<UIAnimation>();
    }

    private void Show() => linkFrom.Show();
    private void Hide() => linkFrom.Hide();

    private void OnEnable()
    {
        if (oppositePhase)
        {
            linkTo.onPreHide += Show;
            linkTo.onPreShow += Hide;
        }
        else
        {
            linkTo.onPreShow += Show;
            linkTo.onPreHide += Hide;
        }
    }

    private void OnDisable()
    {
        if (oppositePhase)
        {
            linkTo.onPreHide -= Show;
            linkTo.onPreShow -= Hide;
        }
        else
        {
            linkTo.onPreShow -= Show;
            linkTo.onPreHide -= Hide;
        }
    }
}
