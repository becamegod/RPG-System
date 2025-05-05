using UnityEngine;

public class EndTurnOnStateExit : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BattleController.Instance.EndTurn(animator.name);
    }
}
