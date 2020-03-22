using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class NamelessPortal : MonoBehaviour
{
    //Vectors for get portal offsets
    public Vector3 getPortalOffset;
    public Vector3 getSummonPortalOffset;
    //Reference for do it bool
    public bool doIt;
    //Portal being teleported to
    public GameObject targetPortal;
    //Portal offsets reference
    public Vector3 summonPortalOffset;
    public Vector3 portalOffset;
    //Reference to portal name string
    public string portalName;
    //Reference for Mr.Nameless cube's rigidbody
    public Rigidbody mrNamelessCube;
    //Reference for gettingchannel bool
    private bool gettingName;
    //Reference to InputField for the channel of the portal
    public InputField getPortalName;
    //Reference to the portal's rigidbody
    public Rigidbody rigidPortal;
    //Reference to portal summoning key string and its default
    public string portalKey = "p";
    // Start is called before the first frame update
    void Start()
    {
        //Initiates the portalOffset Vectors
        summonPortalOffset = new Vector3(0.0f, 0.0f, 3.0f);
        portalOffset = new Vector3(0.0f, 0.0f, -3.0f);
        //Disables portal name input box
        getPortalName.gameObject.SetActive(false);
        //Makes an object for the MrNameLessCube class to get information
        MrNameLessCube mrNameLessCube = new MrNameLessCube();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        if (doIt)
        {
            //Detects if the user is done inputting portal name
            if (Input.GetKeyDown(KeyCode.Return) && !getPortalName.isFocused)
            {
                doIt = false;
                //Stores the portal name  
                portalName = getPortalName.text;
                //Teleports Mr.NamelessCube to the portal selected with offset
                targetPortal = GameObject.Find(portalName);
                mrNamelessCube.position = targetPortal.transform.position + portalOffset;
                //Hides portal name input box
                getPortalName.gameObject.SetActive(false);
            }
        }
        if (Input.GetKeyDown(portalKey))
        {
            //Enables text input field for portal channel
            getPortalName.gameObject.SetActive(true);
            gettingName = true;
        }
        //checks if user is done with making name
        if (gettingName && Input.GetKeyDown(KeyCode.Return) && !getPortalName.isFocused)
        {
            //Makes a portal and sets its and then hides input field
            Instantiate(rigidPortal, mrNamelessCube.position + summonPortalOffset, Quaternion.identity);
            GameObject.FindGameObjectWithTag("isUnactivePortal").name = getPortalName.text;
            GameObject.FindGameObjectWithTag("isUnactivePortal").tag = "isActivePortal";
            getPortalName.gameObject.SetActive(false);
            gettingName = false;
        }
    }
    //Detects for collisions and gets information on the collision
    public void OnCollisionEnter(Collision portalEntering)
    {

        //Tests if the collider is a portal
        if (portalEntering.collider.tag == "isActivePortal")
        {
            doIt = true;
            //Enables input field for portal name
            getPortalName.gameObject.SetActive(true);        
        }
    }
    void SaveData()
    {
        //Saves data
        PlayerPrefs.SetFloat("portal-offset-x", portalOffset.x);
        PlayerPrefs.SetFloat("portal-offset-y", portalOffset.y);
        PlayerPrefs.SetFloat("portal-offset-z", portalOffset.z);
        PlayerPrefs.SetFloat("portal-spawn-offset-x", summonPortalOffset.x);
        PlayerPrefs.SetFloat("portal-spawn-offset-y", summonPortalOffset.y);
        PlayerPrefs.SetFloat("portal-spawn-offset-z", summonPortalOffset.z);
        PlayerPrefs.SetString("portal-key", portalKey);
    }
    void LoadData()
    {
        //Loads data
        Vector3 getPortalOffset = new Vector3(PlayerPrefs.GetFloat("portal-offset-x", 0), PlayerPrefs.GetFloat("portal-offset-y", 0), PlayerPrefs.GetFloat("portal-offset-z", -3));
        portalOffset = getPortalOffset;
        Vector3 getSummonPortalOffset = new Vector3(PlayerPrefs.GetFloat("portal-spawn-offset-x", 0), PlayerPrefs.GetFloat("portal-spawn-offset-y", 0), PlayerPrefs.GetFloat("portal-spawn-offset-z", 3));
        summonPortalOffset = getSummonPortalOffset;
        portalKey = PlayerPrefs.GetString("portal-key", "p");
    }
}
        
   

