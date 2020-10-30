using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieWanderState : StateMachineBehaviour
{
    public float wanderSpeed;
    public float newDestTimer;
    public float newDestRadius;

    private NavMeshAgent agent;
    private float destTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = wanderSpeed;

        Vector3 dest;
        if(GetDestination(animator.transform.position, newDestRadius, out dest))
        {
            agent.SetDestination(dest);
            Debug.DrawRay(dest, Vector3.up, Color.red, 5f);
        }

        destTimer = newDestTimer;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        destTimer += Time.deltaTime;

        if(destTimer >= newDestTimer)
        {
            Vector3 dest;
            if (GetDestination(animator.transform.position, newDestRadius, out dest))
            {
                agent.isStopped = false;
                agent.SetDestination(dest);
                Debug.DrawRay(dest, Vector3.up, Color.red, 10f);
            }
            destTimer = 0;
        }

        if(Vector3.Distance(animator.transform.position, agent.destination) <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            animator.SetBool("isWandering", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isWandering", false);
    }

    private bool GetDestination(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if(NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
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
