using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class synController : MonoBehaviour
{
    float horiMove;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horiMove = Input.GetAxis("Horizontal");
        transform.position = transform.position + new Vector3(horiMove,0,0) * Time.deltaTime;
    }
}