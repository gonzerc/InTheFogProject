using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float wanderTimer;   //time between setting destinations for wandering
    public float wanderRadius;  //radius for random range of destination
    public float attackTimer;   //timer for when enemy can attack player again
    public float chaseTimer;    //timer for updating player's destination when chasing
    public float walkSpeed;     //speed for wandering
    public float chaseSpeed;    //speed for chasing player

    private NavMeshAgent agent;
    private Vector3 targetDestination;
    private GameObject player;
    private int health;
    private int damageAmount;
    private int currencyAmount;
    private float destTimer;
    private float attTimer;

    //enemy states
    private bool wandering;
    private bool chasingPlayer;
    private bool lookingForPlayer;

    public string objName;

    public static List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        objName = "Enemy " + enemies.Count;

        enemies.Add(this.gameObject);
        //Debug.Log("Number of enemies: " + enemies.Count);

        health = 100;
        damageAmount = Random.Range(5, 20);
        currencyAmount = Random.Range(100, 500);
        currencyAmount -= (currencyAmount % 100);

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");


        destTimer = wanderTimer;
        attTimer = 0;
        wandering = true;
        chasingPlayer = false;
        lookingForPlayer = false;
        agent.speed = walkSpeed;

        Debug.Log(this.objName + " : " + this.chasingPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        //wander around
        destTimer += Time.deltaTime;
        attTimer += Time.deltaTime;
        
        //update new 
        if (!lookingForPlayer)
        {
            if (wandering)
            {
                if (destTimer >= wanderTimer)
                {
                    Vector3 dest;
                    if (GetDest(transform.position, wanderRadius, out dest))
                    {
                        targetDestination = dest;
                        //agent.SetDestination(dest);
                        Debug.DrawRay(dest, Vector3.up * 10, Color.blue, 10.0f);
                    }
                    destTimer = 0;
                }

                if((targetDestination - transform.position).magnitude <= 2)
                {
                    agent.isStopped = true;
                }
            }

            //chase after player
            if (chasingPlayer && destTimer >= chaseTimer)
            {
                targetDestination = player.transform.position;
                //agent.SetDestination(player.transform.position);
                destTimer = 0;
            }

            agent.SetDestination(targetDestination);
        }
        else if (lookingForPlayer)
        {
            Debug.DrawRay(targetDestination, Vector3.up * 10, Color.red, 15.0f);
            if((targetDestination - transform.position).magnitude <= 2)
            {
                Debug.Log(objName + " stopped");

                agent.speed = walkSpeed;

                wandering = true;
                lookingForPlayer = false;
            }
        }

        if(attTimer >= attackTimer)
        {
            if ((transform.position - player.transform.position).magnitude <= 2)
            {
                StartCoroutine(DamagePlayer());
                destTimer = 0;
                attTimer = 0;
            }
        }

        if(health <= 0)
        {
            //Debug.Log("Enemies Remaining: " + enemies.Count);
            player.GetComponent<PlayerController>().AddCurrency(currencyAmount);
            enemies.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;
        StartCoroutine(DamageAnimation());
        //Debug.Log(name + " hit. Health: " + health);
    }

    private bool GetDest(Vector3 center, float range, out Vector3 result)
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

    //called from player trigger enter
    public void DetectPlayer()
    {
        lookingForPlayer = false;
        chasingPlayer = true;
        wandering = false;

        agent.speed = chaseSpeed;
        Debug.Log(objName + "chasing player");
    }

    //called from player trigger exit
    public void UndetectPlayer()
    {
        //StartCoroutine(LookForPlayer());
        //chasingPlayer = false;
        //agent.speed = walkSpeed;

        chasingPlayer = false;
        lookingForPlayer = true;

        targetDestination = player.transform.position;
        agent.SetDestination(targetDestination);

        Debug.Log(objName + " looking");
    }

    private IEnumerator LookForPlayer()
    {
        agent.SetDestination(player.transform.position);

        yield return new WaitForSeconds(2f);


    }

    private IEnumerator DamageAnimation()
    {
        Color normalColor = GetComponent<MeshRenderer>().material.color;
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        yield return new WaitForSeconds(0.25f);
        this.gameObject.GetComponent<MeshRenderer>().material.color = normalColor;
    }

    private IEnumerator DamagePlayer()
    {
        Debug.Log("Enemy attack");
        player.GetComponent<PlayerController>().DamagePlayer(damageAmount);
        yield return null;
    }

    public static void PauseGame()
    {
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<NavMeshAgent>().isStopped = true;
        }
    }

    public static void ContinueGame()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    public static void QuitGame()
    {
        foreach(GameObject e in enemies)
        {
            Destroy(e);
        }

        enemies.Clear();
    }
}
