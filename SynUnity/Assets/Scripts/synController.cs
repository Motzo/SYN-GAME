using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class synController : MonoBehaviour
{
    public float speed = 10;
    public float runMultiplier = 1.5f;
    public float jumpheight = 7;
    public float jetPackForce = 1;
    public float jetPackFuel = 10.0f;
    public bool jetPack;
    public bool smoothAirMovement;
    public float smoothAirMovementSpeed = 1;
    public bool doubleJump;
    public float doubleJumpHeight = 5;
    public bool sliding;
    [Tooltip("The amount of time for the sliding to stop in seconds.")]
    public float slideDecel = 1f;
    public bool fallRespawn;
    public bool lastLocationRespawn;
    public float fallRespawnDepth = -10;
    public GameObject bulletPrefab;
    public KeyCode fireKey;
    public KeyCode runKey;
    public float timeBetweenShot = 0.5f;
    public int maxHealth = 5;
    public float respawnTimeLimit;
    public GameObject healthText;
    public Sprite idle;
    public Sprite inAir;
    public AudioSource pew;
    public AudioSource jump;
    public AudioSource splat;
    public AudioSource ded;
    public AudioSource hit;

    bool jumpBool;
    int direction;
    float bulletTimer;
    Vector3 respawnPosition;
    float respawnTimer;
    float horiMove;
    Vector2 velocity;
    Rigidbody2D rigbod;
    int health;
    bool onGround;
    SpriteRenderer sprRen;
    void Start()
    {
        rigbod = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
        bulletTimer = timeBetweenShot;
        direction = 2;
        health = maxHealth;
        sprRen = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        velocity = rigbod.velocity;
        horiMove = Input.GetAxisRaw("Horizontal");
        if(health > 0){
            if(smoothAirMovement){
                if(rigbod.velocity.y == 0){
                    if(Input.GetKey("s") && sliding){ //sliding
                        velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime/slideDecel);
                    }
                    else{
                        velocity.x = horiMove * speed;
                    }
                    
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
            

            if(Input.GetKeyDown("w") || Input.GetKeyDown("up")){
                if(rigbod.velocity.y == 0){
                    velocity.y = jumpheight;
                    jump.Play();
                } 
                else if(jumpBool){
                    jumpBool = false;
                    velocity.y = doubleJumpHeight;
                    jump.Play();
                }
                
            }

            if(Input.GetKey("w") || Input.GetKey("up")){
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

            if(Input.GetKeyDown("w") || Input.GetKeyDown("up")){
                direction = 0;
            }
            if((Input.GetKey("right") || Input.GetKey("d")) && (Input.GetKey("left") || Input.GetKey("a"))){}
            else if(Input.GetKey("right") || Input.GetKey("d")){
                if(Input.GetKey("w") || Input.GetKey("up")){
                    direction = 1;
                }
                else{
                    direction = 2;
                }
            }
            else if(Input.GetKey("left") || Input.GetKey("a")){
                if(Input.GetKey("w") || Input.GetKey("up")){
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

            if(velocity.x > 0.001f){
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(velocity.x < 0.001f){
                GetComponent<SpriteRenderer>().flipX = false;
            }

            if(Input.GetKey(runKey) && !Input.GetKey("s")){
                velocity.x *= runMultiplier;
            }
        }
        else{
            if(respawnTimer == 0){
                ded.Play();
            }
            respawnTimer += Time.deltaTime;
            if(respawnTimer >= respawnTimeLimit){
                health = maxHealth;
                respawnTimer = 0;
                transform.position = respawnPosition;
            }
            velocity.x = 0;
        }

        respawnCheck();

        if(velocity.y == 0){
            sprRen.sprite = idle;
        }
        if(velocity.y != 0){
            onGround = false;
            sprRen.sprite = inAir;
        }

        if(health < 0){
            health = 0;
        }
        healthText.GetComponent<UnityEngine.UI.Text>().text = "Health: " + health;

        rigbod.velocity = velocity;
    }

    void respawnCheck(){
        if(rigbod.velocity.y == 0 && lastLocationRespawn){
            respawnPosition = transform.position;
        }

        if(fallRespawn && transform.position.y < fallRespawnDepth){
            transform.position = respawnPosition;
            velocity = new Vector2(0,0);
        }
    }

    void spawnBullet(float rotation){
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0,0,rotation));
        pew.Play();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "EnemyBullet"){
            health--;
            hit.Play();
        }
    }

    void OnCollisionStay2D(Collision2D other){
        if(velocity.y == 0 && !onGround){
            onGround = true;
            splat.Play();
        }
    }

}