using UnityEngine;
using UnityEngine.AI;

public class idle : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("walk", true);
    }
}
