using UnityEngine;

public class SiblingIndexLock : MonoBehaviour
{
    [SerializeField] ChildrenTransformCallback childrenTransformCallback;
    [SerializeField] int index;

    private void Reset() => childrenTransformCallback = GetComponentInParent<ChildrenTransformCallback>();

    private void Awake() => childrenTransformCallback.OnChanged += SetSiblingIndex;

    private void OnDestroy() => childrenTransformCallback.OnChanged -= SetSiblingIndex;

    private void SetSiblingIndex() => transform.SetSiblingIndex(index);
}
