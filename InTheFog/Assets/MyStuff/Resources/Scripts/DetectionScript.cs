using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionScript : MonoBehaviour
{
    private SphereCollider sphCol;

    private void Start()
    {
        sphCol = GetComponent<SphereCollider>();
    }

    public void ShrinkCollider(float perc)
    {
        sphCol.radius *= perc;
    }

    public void GrowCollider(float perc)
    {
        sphCol.radius /= perc;
    }

    //================================================= Enemy Detection ================================================\\
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().ChasePlayer();
            Debug.Log("Detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().StopChasePlayer();
            Debug.Log("Undetected");
        }
    }
}
