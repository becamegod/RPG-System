using DG.Tweening;
using UnityEngine;

public class HideByTransform : MonoBehaviour
{
    [SerializeField] Transform hideTransform;
    [SerializeField] bool hideOnAwake;
    [SerializeField] float hideShowDuration = 1;
    private Vector3 hidePos;
    private Vector3 showPos;

    private void Awake()
    {
        hidePos = hideTransform.position;
        showPos = transform.position;
        if (hideOnAwake) Hide(0);
    }

    public void Hide(float duration = -1)
    {
        if (duration == -1) duration = hideShowDuration;
        transform.DOMove(hidePos, duration).onComplete += () => gameObject.SetActive(false);
    }

    public void Show()
    {
        transform.DOKill();
        gameObject.SetActive(true);
        transform.DOMove(showPos, hideShowDuration);
    }
}
