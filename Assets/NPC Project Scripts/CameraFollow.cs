using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Transform targetObject; //Select target to follow
    private Vector3 initalOffset; //Initial position of the camera
    private Vector3 cameraPosition; //Position of Camera, constantly updates

    void Start()
    {
        
        initalOffset = new Vector3(0,0, -10);
    }

    void FixedUpdate()
    {

        cameraPosition = targetObject.position + initalOffset; //new position to goto determined by the target objects position + initial position of the camera
        transform.position = cameraPosition; //current position is update
    }
}