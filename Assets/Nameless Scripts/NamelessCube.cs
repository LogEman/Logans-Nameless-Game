using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NamelessCube : MonoBehaviour
{
    //References and defaults to objects and variables
    public Rigidbody cube;
    public Transform MrCube;
    public string summonKey = "c";
    public string removeKey = "r";
    public Vector3 cubeOffset;
    public Vector3 cubePos;
    public Vector3 getOffset;
    void Start()
    {
        //sets offset of the cube in a vector
        cubeOffset = new Vector3(0f, 0f, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(summonKey))
        {
            //summons cube and sets the position of the cube
            Instantiate(cube);
            cube.transform.position = MrCube.transform.position + cubeOffset;
        }

        if (Input.GetKeyDown(removeKey))
        {
            //hits the cube with a ray from the camera from the mouse and destroys the cube that is hit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "isCube")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}