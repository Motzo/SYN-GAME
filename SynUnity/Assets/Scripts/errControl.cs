using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class errControl : MonoBehaviour
{
    public GameObject armsPosition;
    public float attackDistance = 4;
    public GameObject head;
    public float breathDelayTime = 20;
    public GameObject syn;
    public Animator errAnimator;

    float synDistance;
    float breathAngle;
    float breathTimer = 0;
    bool breathAttack;
    void Update()
    {
        if(breathTimer >= breathDelayTime && !breathAttack){
            breathAttack = true;
            breathTimer = 0;
        }
        else if(breathAttack){
            breathAngle = Mathf.Atan2((syn.transform.position.y-head.transform.position.y),(syn.transform.position.x-head.transform.position.x)) * 180 / Mathf.PI;
            head.transform.rotation = Quaternion.Euler(0,0,breathAngle);
            breathTimer += Time.deltaTime;
        }
        else{
            breathTimer += Time.deltaTime;
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
