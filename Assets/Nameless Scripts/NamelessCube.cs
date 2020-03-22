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
        LoadData();
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
    void SaveData()
    {
        //Saves data
        PlayerPrefs.SetFloat("cube-offset-x", cubeOffset.x);
        PlayerPrefs.SetFloat("cube-offset-y", cubeOffset.y);
        PlayerPrefs.SetFloat("cube-offset-z", cubeOffset.z);
        PlayerPrefs.SetString("cube-remove-key", removeKey);
        PlayerPrefs.SetString("cube-summon-key", summonKey);
    }
    void LoadData()
    {
        //Loads data
        Vector3 getOffset = new Vector3(PlayerPrefs.GetFloat("cube-offset-x", 0), PlayerPrefs.GetFloat("cube-offset-y", 0), PlayerPrefs.GetFloat("cube-offset-z", 1));
        cubeOffset = getOffset;
        removeKey = PlayerPrefs.GetString("cube-remove-key", "r");
        summonKey = PlayerPrefs.GetString("cube-summon-key", "c");
    }
    void OnApplicationQuit()
    {
        SaveData();
    }
}

