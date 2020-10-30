using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{
    public float chaseSpeed;

    private ZombieController zController;
    private NavMeshAgent agent;
    private GameObject player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        zController = animator.GetComponent<ZombieController>();
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        agent.speed = chaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (zController.IsChasingPlayer())
        {
            agent.SetDestination(player.transform.position);
            Debug.Log("chasing");
        }
        else if (zController.IsLookingForPlayer())
        {
            Debug.Log("looking");
            Debug.DrawRay(agent.destination, Vector3.up * 5f, Color.green, 15f);
            if(Vector3.Distance(animator.transform.position, agent.destination) < 2f)
            {
                Debug.Log("lost");
                zController.SetLookingForPlayer(false);
                animator.SetBool("isChasing", false);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
