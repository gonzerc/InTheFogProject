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
    private Transform targetDestination;
    private GameObject player;
    private int health;
    private int damageAmount;
    private int currencyAmount;
    private float destTimer;
    private float attTimer;
    private bool chasingPlayer;

    public static List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        enemies.Add(this.gameObject);
        //Debug.Log("Number of enemies: " + enemies.Count);

        health = 100;
        damageAmount = Random.Range(5, 20);
        currencyAmount = Random.Range(100, 500);
        currencyAmount -= (currencyAmount % 100);

        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        destTimer = wanderTimer;
        attTimer = 0;
        chasingPlayer = false;
        agent.speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //wander around
        destTimer += Time.deltaTime;
        attTimer += Time.deltaTime;
        if(destTimer >= wanderTimer && !chasingPlayer)
        {
            Vector3 dest;
            if(GetDest(transform.position, wanderRadius, out dest))
            {
                agent.SetDestination(dest);
                Debug.DrawRay(dest, Vector3.up, Color.blue, 2.0f);
            }
            destTimer = 0;
        }
        
        //chase after player
        if (destTimer >= chaseTimer && chasingPlayer)
        {
            agent.SetDestination(player.transform.position);
            destTimer = 0;
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
    public void ChasePlayer()
    {
        chasingPlayer = true;
        agent.speed = chaseSpeed;
    }

    //called from player trigger exit
    public void StopChasePlayer()
    {
        chasingPlayer = false;
        agent.speed = walkSpeed;
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
}
