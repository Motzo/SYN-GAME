using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class errControl : MonoBehaviour
{
    public GameObject armsPosition;
    public float attackDistance = 4;
    public GameObject head;
    public GameObject bulb;
    int health = 20;
    public int startHealth = 20;
    public LineRenderer breath;
    public GameObject breathBase;
    public Sprite OpenMouth;
    public Sprite ClosedMouth;
    public Sprite DeadHead;
    public SpriteRenderer headSprite;
    public float breathDelayTime = 20;
    public float breathLength = 10;
    public float breathStart = 2;
    public GameObject syn;
    public Animator errAnimator;
    public AudioSource dragonBreath;
    public AudioSource ded;
    public AudioSource hit;

    float synDistance;
    float breathAngle;
    float breathTimer = 0;
    float breathLengthTimer = 0;
    bool breathAttack;
    float deathTimer = 0;
    RaycastHit2D hitLocation;
    private Rigidbody2D rb2d;
    void Start(){
        breath.SetPosition(0, breathBase.transform.position);
        breath.SetPosition(1, breathBase.transform.position);
        health = startHealth;
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(health <= 0 && rb2d.bodyType != RigidbodyType2D.Dynamic){
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0,2));
            errAnimator.SetBool("Dead", true);
            headSprite.sprite = DeadHead;
            ded.Play();
            dragonBreath.Stop();
        }
        else if(health <= 0){
            deathTimer += Time.deltaTime;
            if(deathTimer >= 2.5f){
                SceneManager.LoadScene(4);
            }
        }
        else if(breathTimer >= breathDelayTime && !breathAttack && rb2d.bodyType != RigidbodyType2D.Dynamic){
            breathAttack = true;
            breathTimer = 0;
            breath.SetPosition(1, breathBase.transform.position);
            breath.SetPosition(0, breathBase.transform.position);
            breathLengthTimer = 0;
        }
        else if(breathAttack && rb2d.bodyType != RigidbodyType2D.Dynamic){
            breathAngle = Mathf.Atan2((syn.transform.position.y-head.transform.position.y),(syn.transform.position.x-head.transform.position.x)) * 180 / Mathf.PI;
            head.transform.rotation = Quaternion.Euler(0,0,breathAngle-200);
            if(breathLengthTimer >= breathStart){
                hitLocation = Physics2D.Raycast(head.transform.position, syn.transform.position-head.transform.position);
                breath.SetPosition(1, hitLocation.point);
                breath.SetPosition(0, breathBase.transform.position);
                headSprite.sprite = OpenMouth;
                if(!dragonBreath.isPlaying){
                    dragonBreath.Play();
                }
                if(hitLocation.collider == syn.GetComponent<Collider2D>()){
                    synController.health -= Time.deltaTime;
                }
            }
            else{
                breath.SetPosition(1, breathBase.transform.position);
                breath.SetPosition(0, breathBase.transform.position);
            }
            if(breathLengthTimer >= breathLength){
                breathAttack = false;
                breathLengthTimer = 0;
            }
            breathLengthTimer += Time.deltaTime;
        }
        else if(health > 0){
            breathTimer += Time.deltaTime;
            breath.SetPosition(1, breathBase.transform.position);
            breath.SetPosition(0, breathBase.transform.position);
            headSprite.sprite = ClosedMouth;
        }
        swipeAttack();
        swipeTriggerReset();
    }

    void swipeAttack(){
        synDistance = Vector3.Distance(armsPosition.transform.position, syn.transform.position);
        if(synDistance < attackDistance){ //Checking if syn is close enough
            if(!errAnimator.GetCurrentAnimatorStateInfo(1).IsName("LeftSwipeAttack") && !errAnimator.GetCurrentAnimatorStateInfo(1).IsName("RightSwipeAttack")){ //checks to make sure the attack animations aren't playing
                if(!errAnimator.GetBool("Left Swipe Attack") && !errAnimator.GetBool("Right Swipe Attack")){ // Makes sure that the parameters aren't triggered
                    if(Random.Range(0,2) == 1){ //Chooses a random side
                        errAnimator.SetTrigger("Left Swipe Attack");
                    }
                    else{
                        errAnimator.SetTrigger("Right Swipe Attack");
                    }
                }
            }
        }
    }
    void swipeTriggerReset(){
        if(errAnimator.GetCurrentAnimatorStateInfo(1).IsName("LeftSwipeAttack") || errAnimator.GetCurrentAnimatorStateInfo(1).IsName("RightSwipeAttack")){ //Sets the triggers to false when the animations are playing
            if(errAnimator.GetBool("Left Swipe Attack")){
                errAnimator.SetBool("Left Swipe Attack", false);
            }
            if(errAnimator.GetBool("Right Swipe Attack")){
                errAnimator.SetBool("Right Swipe Attack", false);
            }
        }
    }
    public void RecieveHit(){
        health--;
        hit.Play();
    }
}
