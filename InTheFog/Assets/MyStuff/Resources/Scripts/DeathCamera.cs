using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamera : MonoBehaviour
{
    public float speed;
    public float MAX;

    private void Update()
    {
        if(transform.position.y < MAX && speed > 0)
        {
            this.transform.position += new Vector3(0.0f, speed * Time.deltaTime, -speed * Time.deltaTime);
        }

        if(transform.position.y > MAX - 5 && speed > 0)
        {
            speed -= 0.05f * Time.deltaTime;
        }
    }
}
