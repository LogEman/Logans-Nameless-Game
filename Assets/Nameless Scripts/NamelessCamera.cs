using System.Collections;
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
        //initializing cameraOffset as a vector and setting default
        cameraOffset = new Vector3(0.0f, 0.0f, 0.0f);
        cameraOffset.Set(0f, 1f, -5f);
        LoadData();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {  
        transform.position = MrNameLessCube.position + cameraOffset;
    }
    void OnApplicationQuit()
    {
        SaveData();
    }
    void SaveData()
    {
        //Saves data
        PlayerPrefs.SetFloat("camera-offset-x", cameraOffset.x);
        PlayerPrefs.SetFloat("camera-offset-y", cameraOffset.y);
        PlayerPrefs.SetFloat("camera-offset-z", cameraOffset.z);
    }
    void LoadData()
    {
        //Loads data
        Vector3 getOffset = new Vector3(PlayerPrefs.GetFloat("camera-offset-x", 0), PlayerPrefs.GetFloat("camera-offset-y", 1), PlayerPrefs.GetFloat("camera-offset-z", -5));
        cameraOffset = getOffset;
    }
}
