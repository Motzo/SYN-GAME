using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class errControl : MonoBehaviour
{
    public GameObject armsPosition;
    public float attackDistance = 4;
    public GameObject head;
    public LineRenderer breath;
    public GameObject breathBase;
    public Sprite OpenMouth;
    public Sprite ClosedMouth;
    public SpriteRenderer headSprite;
    public float breathDelayTime = 20;
    public float breathLength = 10;
    public float breathStart = 2;
    public GameObject syn;
    public Animator errAnimator;

    float synDistance;
    float breathAngle;
    float breathTimer = 0;
    float breathLengthTimer = 0;
    bool breathAttack;
    RaycastHit2D hitLocation;
    void Start(){
        breath.SetPosition(0, breathBase.transform.position);
        breath.SetPosition(1, breathBase.transform.position);
    }
    void Update()
    {
        if(breathTimer >= breathDelayTime && !breathAttack){
            breathAttack = true;
            breathTimer = 0;
            breath.SetPosition(1, breathBase.transform.position);
            breathLengthTimer = 0;
        }
        else if(breathAttack){
            breathAngle = Mathf.Atan2((syn.transform.position.y-head.transform.position.y),(syn.transform.position.x-head.transform.position.x)) * 180 / Mathf.PI;
            head.transform.rotation = Quaternion.Euler(0,0,breathAngle-200);
            if(breathLengthTimer >= breathStart){
                hitLocation = Physics2D.Raycast(head.transform.position, syn.transform.position-head.transform.position);
                breath.SetPosition(1, hitLocation.point);
                breath.SetPosition(0, breathBase.transform.position);
                headSprite.sprite = OpenMouth;
                if(hitLocation.collider == syn.GetComponent<Collider2D>()){
                    synController.health -= Time.deltaTime;
                }
            }
            else{
                breath.SetPosition(1, breathBase.transform.position);
            }
            if(breathLengthTimer >= breathLength){
                breathAttack = false;
                breathLengthTimer = 0;
            }
            breathLengthTimer += Time.deltaTime;
        }
        else{
            breathTimer += Time.deltaTime;
            breath.SetPosition(1, breathBase.transform.position);
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
}
