using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float bulletSpeed = 10;
    public float bulletLifeTimeSec = 10;

    float timer;

    void Update()
    {
        transform.Translate(new Vector3(bulletSpeed*Time.deltaTime,0,0));
        timer += Time.deltaTime;
        if(timer >= bulletLifeTimeSec){
            Destroy(gameObject);
        }
    }
}
