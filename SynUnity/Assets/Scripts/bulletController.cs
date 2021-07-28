using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float bulletSpeed = 10;
    public float bulletLifeTimeSec = 10;

    float timer;

    void Start(){
    }
    void Update()
    {
        transform.Translate(new Vector3(bulletSpeed*Time.deltaTime,0,0));
        timer += Time.deltaTime;
        if(timer >= bulletLifeTimeSec){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Syn" && this.gameObject.tag == "EnemyBullet"){
            Destroy(gameObject);
        }
        if(other.tag == "Enemies" && this.gameObject.tag == "SynBullet"){
            Destroy(gameObject);
        }
        if(other.tag == "Bulb"){
            Destroy(gameObject);
        }
        if(other.tag == "obstacle"){
            Destroy(gameObject);
        }
    }
}
