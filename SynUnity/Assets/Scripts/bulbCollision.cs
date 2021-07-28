using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulbCollision : MonoBehaviour
{
    public errControl errScript;
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "SynBullet"){
            errScript.RecieveHit();
        }
    }
}
