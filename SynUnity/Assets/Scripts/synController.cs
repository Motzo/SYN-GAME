using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class synController : MonoBehaviour
{
    public float speed = 10;
    public float jumpheight = 7;
    public float jetPackForce = 1;
    public float jetPackFuel = 10.0f;
    public bool jetPack;
    public bool smoothAirMovement;
    public float smoothAirMovementSpeed = 1;
    public bool doubleJump;
    public float doubleJumpHeight = 5;
    public bool fallRespawn;
    public bool lastLocationRespawn;
    public float fallRespawnDepth = -10;
    public GameObject bulletPrefab;
    public KeyCode fireKey;
    public float timeBetweenShot = 0.5f;

    bool jumpBool;
    int direction;
    float bulletTimer;
    Vector3 respawnPosition;
    float horiMove;
    Vector2 velocity;
    Rigidbody2D rigbod; 
    void Start()
    {
        rigbod = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
        bulletTimer = timeBetweenShot;
        direction = 2;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rigbod.velocity;
        horiMove = Input.GetAxisRaw("Horizontal");

        if(smoothAirMovement){
            if(rigbod.velocity.y == 0){
                velocity.x = horiMove * speed;
            }
            else{
                velocity.x += horiMove * smoothAirMovementSpeed * Time.deltaTime;
                if(velocity.x > speed){
                    velocity.x = speed;
                }
                else if(velocity.x < -speed){
                    velocity.x = -speed;
                }
            }
        }
        else{
            velocity.x = horiMove * speed;
        }
        

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("w") || Input.GetKeyDown("up")){
            if(rigbod.velocity.y == 0){
                velocity.y = jumpheight;
            } 
            else if(jumpBool){
                jumpBool = false;
                velocity.y = doubleJumpHeight;
            }
            
        }

        if(Input.GetKey(KeyCode.Space) || Input.GetKey("w") || Input.GetKey("up")){
            if(rigbod.velocity.y != 0 && jetPack && jetPackFuel > 0){
                rigbod.AddForce(new Vector2(0,jetPackForce*Time.deltaTime));
                jetPackFuel -= Time.deltaTime;
            }
        }

        if(jetPackFuel < 0){
            jetPackFuel = 0;
        }

        if(rigbod.velocity.y == 0 && doubleJump){
            jumpBool = true;
        }

        if(rigbod.velocity.y == 0 && lastLocationRespawn){
            respawnPosition = transform.position;
        }

        if(fallRespawn && transform.position.y < fallRespawnDepth){
            transform.position = respawnPosition;
            velocity = new Vector2(0,0);
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("w") || Input.GetKeyDown("up")){
            direction = 0;
        }
        if((Input.GetKey("right") || Input.GetKey("d")) && (Input.GetKey("left") || Input.GetKey("a"))){}
        else if(Input.GetKey("right") || Input.GetKey("d")){
            if(Input.GetKey(KeyCode.Space) || Input.GetKey("w") || Input.GetKey("up")){
                direction = 1;
            }
            else{
                direction = 2;
            }
        }
        else if(Input.GetKey("left") || Input.GetKey("a")){
            if(Input.GetKey(KeyCode.Space) || Input.GetKey("w") || Input.GetKey("up")){
                direction = -1;
            }
            else{
                direction = -2;
            }
        }
        
        if(bulletTimer < timeBetweenShot){
            bulletTimer += Time.deltaTime;
        }
        else if(Input.GetKeyDown(fireKey)){
            if(direction == 2)
                spawnBullet(0);
            else if(direction == 1)
                spawnBullet(45);
            else if(direction == 0)
                spawnBullet(90);
            else if(direction == -1)
                spawnBullet(135);
            else if(direction == -2)
                spawnBullet(180);
            bulletTimer = 0;
        }

        rigbod.velocity = velocity;
    }

    void spawnBullet(float rotation){
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0,0,rotation));
    }
}