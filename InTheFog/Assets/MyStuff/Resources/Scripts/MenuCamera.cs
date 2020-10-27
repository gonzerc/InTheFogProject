using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public float speed;

    void Update()
    {
        if(transform.position.z < 80)
        {
            this.transform.position += new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
        }

        if(transform.position.z > 70 && speed > 0)
        {
            speed -= 0.05f * Time.deltaTime;
        }
    }
}
