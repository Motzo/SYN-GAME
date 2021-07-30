using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Animator healthBarAnimator;
    public Animator errAnimator;
    public Animator animatorController;
    public AudioSource pew;
    public AudioSource jump;
    public AudioSource splat;
    public AudioSource ded;
    public AudioSource hit;
    public AudioSource respawn;
    public AudioSource heal;

    bool jumpBool;
    int direction;
    float bulletTimer;
    Vector3 respawnPosition;
    float respawnTimer;
    float horiMove;
    Vector2 velocity;
    Rigidbody2D rigbod;
    public static float health;
    bool onGround;
    SpriteRenderer sprRen;
    int lives = 3;
    void Start()
    {
        rigbod = GetComponent<Rigidbody2D>(); //Getting the rigidbody component

        respawnPosition = transform.position; //Setting the respawn position to the starting location
        bulletTimer = timeBetweenShot; //Setting the bullet timer to be ready for firing
        direction = -2; // Setting the bullet direction to the default direction
        health = maxHealth; //Setting the health to the max health
        sprRen = GetComponent<SpriteRenderer>(); // Getting the comonent of the Sprite renderer
    }
    
    void Update()
    {
        velocity = rigbod.velocity; //Setting the velocity variable to the rigidbody velocity
        horiMove = Input.GetAxisRaw("Horizontal"); //Horizontal Axis

        healthBarAnimator.SetFloat("Health", health);

        if(health > 0){
            xMovement();
            JumpControl();
            JetPackControl();

            if(rigbod.velocity.y == 0 && doubleJump){
                jumpBool = true;
            }
            
            CheckBulletFire();
        }
        else{
            DeathAction();
        }

        respawnCheck();
        AnimationControl();
        UpdateHealthText();

        if(lives <= 0){
            SceneManager.LoadScene(5);
        }

        rigbod.velocity = velocity; //Set velocity to 
    }

    void respawnCheck(){
        if(rigbod.velocity.y == 0 && lastLocationRespawn){
            respawnPosition = transform.position;
        }

        if(fallRespawn && transform.position.y < fallRespawnDepth){
            transform.position = respawnPosition;
            lives--;
            health = maxHealth;
            animatorController.SetBool("Dead", false);
            heal.Play();
            velocity = new Vector2(0,0);
        }
    }
    void AnimationControl(){
        if(velocity.x > 0.001f){
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(velocity.x < -0.001f){
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if(velocity.y < -0.0001f){
            animatorController.SetInteger("yVelocity", -1);
        }
        else if(velocity.y > 0.0001f){
            animatorController.SetInteger("yVelocity", 1);
        }
        else{
            animatorController.SetInteger("yVelocity", 0);
        }

        if((velocity.x > 0 || velocity.x < 0)){
            animatorController.SetBool("Walking", true);
        }
        else{
            animatorController.SetBool("Walking", false);
        }
    }
    void xMovement(){
        if(smoothAirMovement){
            if(rigbod.velocity.y == 0){
                if((Input.GetKey("s") || Input.GetKey("down")) && sliding){ //sliding
                    velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime/slideDecel);
                    animatorController.SetBool("Sliding", true);
                }
                else{
                    velocity.x = horiMove * speed;
                    animatorController.SetBool("Sliding", false);
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
            animatorController.SetBool("Sliding", false);
        }
        if(Input.GetKey(runKey) && !Input.GetKey("s")){
            velocity.x *= runMultiplier;
        }
    }
    void JumpControl(){
        if(Input.GetKeyDown("w")/* || Input.GetKeyDown("up")*/){
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
        if(rigbod.velocity.y == 0 && doubleJump){ //Double jump reset
            jumpBool = true;
        }
    }
    void JetPackControl(){
        if(Input.GetKey("w")/*  || Input.GetKey("up")*/){
            if(rigbod.velocity.y != 0 && jetPack && jetPackFuel > 0){
                rigbod.AddForce(new Vector2(0,jetPackForce*Time.deltaTime));
                jetPackFuel -= Time.deltaTime;
            }
        }

        if(jetPackFuel < 0){
            jetPackFuel = 0;
        }
    }
    void UpdateHealthText(){
        if(health < 0){
            health = 0;
        }
        healthText.GetComponent<UnityEngine.UI.Text>().text = "Health: " + health;
    }
    void CheckFireDirection(){
        // if(/*Input.GetKeyDown("w") || */Input.GetKeyDown("up")){
        //     direction = 0;
        // }
        // if((Input.GetKey("right")/* || Input.GetKey("d")*/) && (Input.GetKey("left")/* || Input.GetKey("a")*/)){}
        // else if(Input.GetKey("right")/* || Input.GetKey("d")*/){
        //     if(/*Input.GetKey("w") || */Input.GetKey("up")){
        //         direction = 1;
        //     }
        //     else{
        //         direction = 2;
        //     }
        // }
        // else if(Input.GetKey("left")/* || Input.GetKey("a")*/){
        //     if(/*Input.GetKey("w") || */Input.GetKey("up")){
        //         direction = -1;
        //     }
        //     else{
        //         direction = -2;
        //     }
        // }
        if(Input.GetKeyDown("up")){
            if(Input.GetKey("right") && Input.GetKey("left")){
                direction = 0;
            }
            // else if(Input.GetKey("right")){
            //     direction = 1;
            // }
            // else if(Input.GetKey("left")){
            //     direction = -1;
            // }
            else{
                direction = 0;
            }
            spawnBullet(direction);
        }
        else if(Input.GetKeyDown("right")){
            // if(Input.GetKey("up")){
            //     direction = 1;
            // }
            // else{
                direction = 2;
            // }
            spawnBullet(direction);
        }
        else if(Input.GetKeyDown("left")){
            // if(Input.GetKey("up")){
            //     direction = -1;
            // }
            // else{
                direction = -2;
            // }
            spawnBullet(direction);
        }
    }
    void CheckBulletFire(){
        if(bulletTimer < timeBetweenShot){
            bulletTimer += Time.deltaTime;
        }
        else{
            animatorController.SetBool("Firing", false);
            animatorController.SetBool("FiringUp", false);
            //if(velocity.y < 0.0001f && velocity.y > -0.0001f){
                CheckFireDirection();
            //}
        }
    }
    void spawnBullet(int direction){
        float rotation = 180;
        //if(Input.GetKeyDown(fireKey)){
            if(direction == 2){
                rotation = 0;
                animatorController.SetBool("Firing", true);
            }
            else if(direction == 1){
                rotation = 45;
                animatorController.SetBool("Firing", true);
            }
            else if(direction == 0){
                rotation = 90;
                animatorController.SetBool("FiringUp", true);
            }
            else if(direction == -1){
                rotation = 135;
                animatorController.SetBool("Firing", true);
            }
            else if(direction == -2){
                rotation = 180;
                animatorController.SetBool("Firing", true);
            }
            bulletTimer = 0;
        //}
        Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0,0,rotation));
        pew.Play();
        
    }
    void DeathAction(){
        if(respawnTimer == 0){
            ded.Play();
            animatorController.SetBool("Dead", true);
        }
        respawnTimer += Time.deltaTime;
        if(respawnTimer >= respawnTimeLimit){
            health = maxHealth;
            lives--;
            respawnTimer = 0;
            animatorController.SetBool("Dead", false);
            transform.position = respawnPosition;
            respawn.Play();
        }
        velocity.x = 0;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "EnemyBullet"){
            health--;
            hit.Play();
        }
        if(other.tag == "ErrHand"){
            health--;
            hit.Play();
        }
        if(other.tag == "Health Item"){
            health = maxHealth;
            respawn.Play();
            Destroy(other.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D other){
        if(velocity.y == 0 && !onGround){
            onGround = true;
            splat.Play();
        }
    }

}