using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unisave;
public class MrNameLessCube : UnisaveLocalBehavior
{
    public Vector3 exitPortal;
    //reference to how long until hunger decreases health
    public float decreaseHealth = 50f;
    private float decreaseHealthCurrent = 0f;
    //reference to how long an action is done until hunger value is lowered
    public float decreaseHungerCurrent = 0f;
    public float decreaseHunger = 100f;
    //reference to spawnpoint
    public Vector3 spawnPoint;
    //reference for health values and defaults
    [SavedAs("health")]
    public float health = 21f;
    [SavedAs("hunger")]
    public float hunger = 21f;
    [SavedAs("max-hunger")]
    public float maxHunger = 21f;
    [SavedAs("max-health")]
    public float maxHealth = 21f;
    [SavedAs("alive")]
    public bool alive = true;
    //reference to refresh gravity bool and default value
    [SavedAs("refresh-gravity")]
    public bool refreshGravity = false;
    //reference to jump reset bool
    private bool jumpReset;
    //references gravity for Nameless Planet and weight for Mr.NameLess Cube
    [SavedAs("gravity")]
    public float gravity = 1f;
    [SavedAs("weight")]
    public float weight = 1f;
    [SavedAs("gravity-force")]
    private float gravityForce = 0f;
    //references computercheck bool
    [SavedAs("computer-check")]
    private bool computerCheck;
    //references for movement keys for Mr.Nameless Cube and their defaults and test bools for said keys
    [SavedAs("left-key")]
    public string leftKey = "a";
    private bool leftKeyPressed;
    [SavedAs("right-key")]
    public string rightKey = "d";
    private bool rightKeyPressed;
    [SavedAs("forward-key")]
    public string forwardKey = "w";
    private bool forwardKeyPressed;
    [SavedAs("backward-key")]
    public string backwardKey = "s";
    private bool backwardKeyPressed;
    [SavedAs("jump-key")]
    public string jumpKey = "j";
    private bool jumpKeyValid;
    //reference for max jump frames and its default
    [SavedAs("max-jump")]
    public float maxJump = 1f;
    private float localMaxJump;
    private int currentJump;
    //references for how fast Mr.NameLessCube can move on the X, Y, and Z axis and their defaults
    [SavedAs("xforce")]
    public float xForce = 500f;
    [SavedAs("yforce")]
    public float yForce = 1350f;
    [SavedAs("zforceS")]
    public float zForce = 500f;
    //references rigidbody of Mr.Nameless Cube and Nameless food
    [SavedAs("mrcube")]
    public Rigidbody mrcube;
    [SavedAs("food")]
    public Rigidbody food;
    //food generation timer
    public float foodTime = 10f;
    [SavedAs("current-food-time")]
    private float currentFoodTime;
    //reference for the text UI element
    [SavedAs("stats-text")]
    public Text statsText;
    //reference to the  
    NamelessPortal portal;
    // Start is called before the first frame update
    public void Start()
    {
        NamelessPortal portal = new NamelessPortal();
        //Initiating the Vector for Mr.Nameless cube's spawn point
        spawnPoint = new Vector3(0, 1, 0);
        //Initiating the bool for testing Mr.Nameless Cube movement keys
        leftKeyPressed = false;
        rightKeyPressed = false;
        forwardKeyPressed = false;
        backwardKeyPressed = false;
        jumpKeyValid = false;
        //Initiating bool for computer power check
        computerCheck = false;
        //Initiating the int value for current jump variable
        currentJump = 0;
        //Initiating the value of refresh gravity
        refreshGravity = true;
        //Initiating Mr.Nameless cube's health values
        
        health = 21f;
        hunger = 21f;
        alive = true;

    }
    // Update is called once per frame
    public void Update()
    {

        //Shows the stats of Mr.Nameless Cube on screen
        statsText.text = "Health: " + health.ToString() + " Hunger: " + hunger.ToString();
        //Kills Mr.Nameless Cube if he falls into the void
        if (mrcube.position.y < 0)
        {
            health = 0;
        }
        //tests if Mr.Nameless cube is still alive
        if (health > 0f)
        {
            alive = true;
        }
        else
        {
            alive = false;
        }
        if (!alive)
        {
            mrcube.position = spawnPoint;
            alive = true;
            health = 21f;
            hunger = 21f;
        }
        //makes sure refresh gravity is only called once or when refreshed again
        if (refreshGravity)
        {
            //The force of gravity that is applied to Mr.Nameless Cube is calculated here
            gravityForce = gravity * weight;
            //Lowers yForce based on gravityforce strength
            yForce = yForce - gravityForce;
            refreshGravity = false;
        }
        //adjusts variables based on your computer power
        if (!computerCheck)
        {
            foodTime = foodTime / Time.deltaTime;
            decreaseHealth = decreaseHealth / Time.deltaTime;
            decreaseHunger = decreaseHunger / Time.deltaTime;
            localMaxJump = maxJump / Time.deltaTime;
            computerCheck = true;
        }
        //timer for summoning food objects
        if (currentFoodTime > 0f)
        {
            --currentFoodTime;

        }
        else
        {
            currentFoodTime = foodTime;
            Instantiate(food, new Vector3(Random.Range(-1000, 1000), 2, Random.Range(-1000, 1000)), Quaternion.identity);
        }
        if (decreaseHungerCurrent <= 0f && hunger > 0f)
        {
            decreaseHungerCurrent = decreaseHunger;
            hunger--;
        }
        if (decreaseHealthCurrent <= 0f && health > 0f)
        {
            decreaseHealthCurrent = decreaseHealth;
            health--;
        }
        if (hunger <= 0)
        {
            decreaseHealthCurrent--;
        }
        //tests if the left key is pressed and sets the bool based on that
        if (Input.GetKey(leftKey))
        {
            leftKeyPressed = true;
            decreaseHungerCurrent--;
        }
        else
        {
            leftKeyPressed = false;
        }
        //tests if the right key is pressed and sets the bool based on that
        if (Input.GetKey(rightKey))
        {
            rightKeyPressed = true;
            decreaseHungerCurrent--;
        }
        else
        {
            rightKeyPressed = false;
            decreaseHungerCurrent--;
        }
        //tests if the forward key is pressed and sets the bool based on that
        if (Input.GetKey(forwardKey))
        {
            forwardKeyPressed = true;
            decreaseHungerCurrent--;
        }
        else
        {
            forwardKeyPressed = false;
        }
        //tests if the backward key is pressed and sets the bool based on that
        if (Input.GetKey(backwardKey))
        {
            backwardKeyPressed = true;
            decreaseHungerCurrent--;
        }
        else
        {
            backwardKeyPressed = false;
        }
        //tests if the jump key is pressed then tests if Mr.Nameless Cube can jump then sets the bool based on that
        if (Input.GetKey(jumpKey) && currentJump <= localMaxJump)
        {
            jumpKeyValid = true;
            currentJump++;
            decreaseHungerCurrent--;
        }
        else
        {
            jumpKeyValid = false;
        }
        //resets currentJump and jumpReset
        if (jumpReset)
        {
            currentJump = 0;
            jumpReset = false;
        }


    }
    //Put things for physics here
    void FixedUpdate()
    {
        //Adds force to Mr.Nameless cube in X,Y, and Z axis based on if keys are pressed and valid
        if (leftKeyPressed)
        {
            mrcube.AddForce(-xForce * Time.deltaTime, 0, 0);
        }
        if (rightKeyPressed)
        {
            mrcube.AddForce(xForce * Time.deltaTime, 0, 0);
        }
        if (forwardKeyPressed)
        {
            mrcube.AddForce(0, 0, zForce * Time.deltaTime);
        }
        if (backwardKeyPressed)
        {
            mrcube.AddForce(0, 0, -zForce * Time.deltaTime);
        }
        if (jumpKeyValid)
        {
            mrcube.AddForce(0, yForce * Time.deltaTime, 0);
        }
    }
    //gets info on collisions with Mr.Nameless Cube
    private void OnCollisionEnter(Collision collisionInfo)
    {
        //checks if the thing Mr.Nameless cube collided with has the canResetJump tag
        if (collisionInfo.collider.tag == "canResetJump")
        {
            //sets jumpReset bool to true
            jumpReset = true;
        }
        if (collisionInfo.collider.tag == "isFood")
        {
            //determines what to do with food
            if (hunger <= 20f && health < 20f)
            {
                health++;
            }
            else
            {
                hunger = hunger + maxHunger - hunger;
            }

            if (hunger <= 20)
            {
                Destroy(collisionInfo.gameObject);
            }
        }
    }
}
