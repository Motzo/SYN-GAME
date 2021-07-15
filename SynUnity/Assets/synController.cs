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
    public bool doublejump;

    private bool jumpBool;
    float horiMove;
    Vector2 velocity;
    Rigidbody2D rigbod; 
    void Start()
    {
        rigbod = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rigbod.velocity;
        horiMove = Input.GetAxisRaw("Horizontal");
        velocity.x = horiMove * speed;

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("w") || Input.GetKeyDown("up")){
            if(rigbod.velocity.y == 0){
                velocity.y = jumpheight;
            } 
            else if(jumpBool){
                jumpBool = false;
                velocity.y = jumpheight;
            }
            
        }

        if(Input.GetKey(KeyCode.Space) || Input.GetKey("w") || Input.GetKey("up")){
            if(rigbod.velocity.y != 0 && jetPack && jetPackFuel > 0){
                rigbod.AddForce(new Vector2(0,jetPackForce));
                jetPackFuel -= Time.deltaTime;
            }
        }

        if(jetPackFuel < 0){
            jetPackFuel = 0;
        }

        if(rigbod.velocity.y == 0 && doublejump){
            jumpBool = true;
        }

        rigbod.velocity = velocity;
    }
}