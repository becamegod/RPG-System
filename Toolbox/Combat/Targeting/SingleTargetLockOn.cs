using UnityEngine;

public class SingleTargetLockOn : MonoBehaviour
{
    [SerializeField] BaseTargetDetector targetDetector;

    bool Visible
    {
        get => transform.localScale != Vector3.zero;
        set => transform.localScale = (value) ? Vector3.one : Vector3.zero;
    }

    private void Update()
    {
        var target = targetDetector.Target;
        if (target is not null)
        {
            Visible = true;
            transform.position = target.bounds.center;
        }
        else Visible = false;
    }
}