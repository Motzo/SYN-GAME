using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public float smoothSpeed = 0.1f;
    public GameObject syn;
    public Vector3 offset = new Vector3(0,1,-10);
    public bool lockY;
    public float lockedYOffset = -1;

    Vector3 finalPosition;
    void FixedUpdate()
    {
        finalPosition = syn.transform.position + offset;
        if(lockY)
            finalPosition.y = lockedYOffset;
        transform.position = Vector3.Lerp(transform.position, finalPosition, smoothSpeed);
    }
}
