using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootMotionTransform : MonoBehaviour
{
    [SerializeField] Transform subject;

    Animator animator;

    private void Awake() => animator = GetComponent<Animator>();

    private void OnAnimatorMove()
    {
        subject.position += animator.deltaPosition;
        subject.forward = animator.deltaRotation * subject.forward;
    }
}
