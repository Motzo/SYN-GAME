using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public float timeBetweenShot = 0.5f;
    float timer;
    public GameObject Syn;
    public GameObject bulletPrefab;
    public int maxHealth = 3;
    public AudioSource pew;
    public AudioSource ded;

    float bulletAngle;
    int health;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if(timer < timeBetweenShot)
            timer += Time.deltaTime;
        else if(timer >= timeBetweenShot){
            shootBullet();
        }

        if(health <= 0){
            ded.Play();
            DestroyImmediate(gameObject);
        }
    }
    
    void shootBullet()
    {
        bulletAngle = Mathf.Atan2((Syn.transform.position.y-transform.position.y),(Syn.transform.position.x-transform.position.x)) * 180 / Mathf.PI;
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0,0,bulletAngle));
        timer = 0;
        pew.Play();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "SynBullet"){
            health--;
        }
    }
}
