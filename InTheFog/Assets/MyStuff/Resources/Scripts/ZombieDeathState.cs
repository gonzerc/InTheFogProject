using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieDeathState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("this guy just died");

        animator.GetComponent<NavMeshAgent>().isStopped = true;

        Destroy(animator.gameObject, stateInfo.length);
    }
}
