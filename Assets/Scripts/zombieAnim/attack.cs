using UnityEngine;

public class attack : StateMachineBehaviour
{
    Transform player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       //animator.transform.LookAt(player);

        float distance = Vector3.Distance(animator.transform.position, player.position);

        if (distance > 3)
            animator.SetBool("attack", false);

        if (distance > 15)
            animator.SetBool("walk", true);

    }
}
