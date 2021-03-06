﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NamelessCamera : MonoBehaviour
{
    public Transform MrNameLessCube;
    public Vector3 cameraOffset;
    public Vector3 getOffset;
    // Start is called before the first frame update
    void Start()
    {
        MrNameLessCube = GameObject.FindGameObjectWithTag("isMrCube").transform;
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