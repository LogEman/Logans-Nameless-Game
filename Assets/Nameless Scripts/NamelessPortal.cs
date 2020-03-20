using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class NamelessPortal : MonoBehaviour
{

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
        //disables portal name input
        getPortalName.gameObject.SetActive(false);
        //Makes an object for the MrNameLessCube class to get information
        MrNameLessCube mrNameLessCube = new MrNameLessCube();
    }

    // Update is called once per frame
    void Update()
    {
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
            Instantiate(rigidPortal, mrNamelessCube.position, Quaternion.identity);
            rigidPortal.name = getPortalName.text;
            getPortalName.gameObject.SetActive(false);
            gettingName = false;
            }
        }
        
    }

