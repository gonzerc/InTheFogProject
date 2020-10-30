using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : StateMachineBehaviour
{
    private int damageAmount;

    private PlayerController player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<PlayerController>();

        damageAmount = Random.Range(5, 15);

        player.DamagePlayer(damageAmount);
    }
}
