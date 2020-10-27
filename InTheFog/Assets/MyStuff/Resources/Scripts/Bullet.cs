using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public int damage;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 100 * bulletSpeed);
    }


    private void Update()
    {
        if(transform.position.y < 0)
        {
            Destroy(this.gameObject);
            //Debug.Log("Destroy by ground");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            //Debug.Log("Hit " + other.name);
            Destroy(this.gameObject);
        }

    }
}
