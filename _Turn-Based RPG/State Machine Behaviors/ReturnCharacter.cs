using UnityEngine;

public class ReturnCharacter : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var movement = animator.GetComponentInParent<CharacterMovement>();
        movement.Return();
        animator.SetLayerWeight(1, 1);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetLayerWeight(1, 0);
    }
}
