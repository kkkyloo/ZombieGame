using UnityEngine;
using UnityEngine.AI;

public class run : StateMachineBehaviour
{
   NavMeshAgent agent;
   Transform player;
   public static float speed = 9f;
   float attackRange = 3.9f;

   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      agent = animator.GetComponent<NavMeshAgent>();
      
      player = GameObject.FindGameObjectWithTag("Player").transform;
   }

   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
      agent.speed = speed;

      agent.SetDestination(player.position);
      float distance = Vector3.Distance(animator.transform.position, player.position);

      if (distance < attackRange)
       animator.SetBool("attack", true);

       if (distance > 10)
           animator.SetBool("attack", false);

   }
}
