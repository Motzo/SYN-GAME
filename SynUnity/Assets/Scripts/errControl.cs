using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class errControl : MonoBehaviour
{
    public GameObject syn;
    public Animator errAnimator;

    float synDistance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        synDistance = Vector3.Distance(transform.position, syn.transform.position);
        
        if(synDistance < 8/* && !errAnimator.GetCurrentAnimatorClipInfo(1).IsName("SwipeAttack")*/){
            errAnimator.SetTrigger("Swipe Attack");
        }
    }
}
