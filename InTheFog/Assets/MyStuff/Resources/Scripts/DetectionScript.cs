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

    public float GetCurrentRadius()
    {
        return sphCol.radius;
    }

    public void ChangeRadius(float newRadius)
    {
        sphCol.radius = newRadius;
    }

    //================================================= Enemy Detection ================================================\\
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        other.gameObject.GetComponent<ZombieController>().DetectPlayer();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        other.gameObject.GetComponent<ZombieController>().UndetectPlayer();
    //    }
    //}
}
