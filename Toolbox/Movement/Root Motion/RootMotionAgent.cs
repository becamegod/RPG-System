using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class RootMotionAgent : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    Animator animator;

    private void Reset() => agent = GetComponentInParent<NavMeshAgent>();

    private void Awake() => animator = GetComponent<Animator>();

    private void OnAnimatorMove()
    {
        if (!animator.GetBool("Move")) return;
        var velocity = animator.deltaPosition.magnitude;
        if (velocity > 0) agent.speed = velocity / Time.deltaTime;
        agent.angularSpeed = animator.deltaRotation.eulerAngles.magnitude / Time.deltaTime;
    }
}
