using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public static List<GameObject> zombies = new List<GameObject>();

    private string zombieIndex;
    private int zombieHealth;
    private int currencyAmount;

    public float attackWait;
    private float attackTimer;

    private bool chasingPlayer;
    private bool lookingForPlayer;
    private bool dying;

    private Animator animator;
    private GameObject player;

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        zombieIndex = "Zombie " + zombies.Count;
        zombies.Add(this.gameObject);

        zombieHealth = 100;

        currencyAmount = Random.Range(100, 500);
        currencyAmount -= (currencyAmount % 100);

        attackTimer = attackWait;
        chasingPlayer = false;
        lookingForPlayer = false;
        dying = false;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if(Vector3.Distance(transform.position, player.transform.position) <= 2) {
            if (attackTimer >= attackWait)
            {
                animator.SetTrigger("attack");
                attackTimer = 0;
            }
        }

        if(this.zombieHealth <= 0 && !dying)
        {
            dying = true;
            StartCoroutine(ActivateZombieDeath());
        }
    }

    public bool IsChasingPlayer()
    {
        return chasingPlayer;
    }

    public bool IsLookingForPlayer()
    {
        return lookingForPlayer;
    }

    public void DetectPlayer()
    {
        chasingPlayer = true;
        animator.SetBool("isChasing", true);
        player.GetComponent<PlayerController>().SetZombieDetected(animator.gameObject);
    }

    public void UndetectPlayer()
    {
        chasingPlayer = false;
        lookingForPlayer = true;
        player.GetComponent<PlayerController>().SetZombieSearching(animator.gameObject);
    }

    public void LostPlayer()
    {
        lookingForPlayer = false;
        player.GetComponent<PlayerController>().SetZombieLost(animator.gameObject);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(zombieIndex + " took " + damage + " damage.");
        zombieHealth -= damage;
        StartCoroutine(FindObjectOfType<UIController>().DisplayHitMarker());

        //StartCoroutine(DamageAnimation());
    }


    //private IEnumerator DamageAnimation()
    //{
    //    Color normalColor = GetComponent<MeshRenderer>().material.color;
    //    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

    //    yield return new WaitForSeconds(0.25f);
    //    this.gameObject.GetComponent<MeshRenderer>().material.color = normalColor;
    //}



    private IEnumerator ActivateZombieDeath()
    {
        Debug.Log(zombieIndex + " died");
        player.GetComponent<PlayerController>().SetZombieLost(this.gameObject);
        zombies.Remove(this.gameObject);

        int animInt = Random.Range(0, 2);
        animator.SetInteger("fallAnim", animInt);
        animator.SetTrigger("dead");

        player.GetComponent<PlayerController>().AddCurrency(currencyAmount);
        yield return null;
    }





    //==================================Player detection functions=================================\

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerDetection"))
        {
            DetectPlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerDetection"))
        {
            UndetectPlayer();
        }
    }




    //===========================game controls - on pause, continue, and quit==============================\\


    public static void PauseGame()
    {
        foreach (GameObject zombie in zombies)
        {
            zombie.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }

    public static void ContinueGame()
    {
        foreach (GameObject zombie in zombies)
        {
            zombie.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    public static void QuitGame()
    {
        foreach (GameObject zombie in zombies)
        {
            Destroy(zombie);
        }

        zombies.Clear();
    }
}
