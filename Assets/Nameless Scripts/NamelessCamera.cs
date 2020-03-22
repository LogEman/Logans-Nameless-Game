using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unisave;
public class NamelessCamera : UnisaveLocalBehavior
{
    public Transform MrNameLessCube;
    [SavedAs("camera-offset")]
    public Vector3 cameraOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        //initializing cameraOffset as a vector and setting default
        cameraOffset = new Vector3(0.0f, 0.0f, 0.0f);
        cameraOffset.Set(0f, 1f, -5f);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
   
        transform.position = MrNameLessCube.position + cameraOffset;
    }
}
