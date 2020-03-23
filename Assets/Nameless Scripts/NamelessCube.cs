using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NamelessCube : MonoBehaviour
{
    //References and defaults to objects and variables
    public Rigidbody cube;
    public Transform MrCube;
    public string removeKey = "r";
    private int cubeNum;
    private bool CubeNumUsed;
    private bool cubesLoaded;
    void Start()
    {
        //sets offset of the cube in a vector
        LoadData();
    }
    // Update is called once per frame
    void Update()
        {
        if (Input.GetKeyDown("o"))
        {
            SaveData();
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
    void SaveData()
    {
      
    }
    void LoadData()
    {
       
            //Loads data
            removeKey = PlayerPrefs.GetString("cube-remove-key", "r");
        }
        
    
    void OnApplicationQuit()
    {
        SaveData();
    }
}

