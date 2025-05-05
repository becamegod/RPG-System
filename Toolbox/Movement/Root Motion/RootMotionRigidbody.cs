using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RootMotionRigidbody : MonoBehaviour
{
    [SerializeField] Rigidbody body;

    Animator animator;

    private void Reset() => body = GetComponentInParent<Rigidbody>();

    private void Awake() => animator = GetComponent<Animator>();

    private void OnAnimatorMove() => body.linearVelocity = animator.velocity;
}
