using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class errControl : MonoBehaviour
{
    public float attackDistance = 8;
    public GameObject syn;
    public Animator errAnimator;

    float synDistance;
    void Update()
    {
        synDistance = Vector3.Distance(transform.position, syn.transform.position);

        if(synDistance < attackDistance){
            if(!errAnimator.GetCurrentAnimatorStateInfo(1).IsName("LeftSwipeAttack") && !errAnimator.GetCurrentAnimatorStateInfo(1).IsName("RightSwipeAttack")){
                if(!errAnimator.GetBool("Left Swipe Attack") && !errAnimator.GetBool("Right Swipe Attack")){
                    if(Random.Range(0,2) == 1){
                        errAnimator.SetTrigger("Left Swipe Attack");
                    }
                    else{
                        errAnimator.SetTrigger("Right Swipe Attack");
                    }
                }
            }
        }
    }
}
