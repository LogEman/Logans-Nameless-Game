using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MrNameLessCube : MonoBehaviour
{
    public GameObject enemy;
    private int enemyCountMax;
    private int newEnemyCountMax;
    public string attackKey = "b";
    public float attackDamage = 1f;
    public double healthScore;
    public float dps = 3;
    public Button reset;
    public Rigidbody cube;
    private GameObject[] cubes;
    private GameObject[] enemies;
    private int cubeCountMax;
    public Vector3 getPos;
    public Vector3 getSpawn;
    //reference vector to exit portal
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
    public float health = 21f;
    public float hunger = 21f;
    public float maxHunger = 21f;
    public float maxHealth = 21f;
    public bool alive = true;
    //reference to refresh gravity bool and default value
    public bool refreshGravity = false;
    //reference to jump reset bool
    private bool jumpReset;
    //references gravity for Nameless Planet and weight for Mr.NameLess Cube
    public float gravity = 1f;
    public float weight = 1f;
    private float gravityForce = 0f;
    //references computercheck bool
    private bool computerCheck;
    //references for movement keys for Mr.Nameless Cube and their defaults and test bools for said keys
    public string leftKey = "a";
    private bool leftKeyPressed;
    public string rightKey = "d";
    private bool rightKeyPressed;
    public string forwardKey = "w";
    private bool forwardKeyPressed;
    public string backwardKey = "s";
    private bool backwardKeyPressed;
    public string jumpKey = "j";
    private bool jumpKeyValid;
    //reference for max jump frames and its default
    public float maxJump = 1f;
    private float localMaxJump;
    private int currentJump;
    //references for how fast Mr.NameLessCube can move on the X, Y, and Z axis and their defaults
    public float xForce = 500f;
    public float yForce = 1350f;
    public float zForce = 500f;
    //references rigidbody of Mr.Nameless Cube and Nameless food
    public Rigidbody mrcube;
    public Rigidbody food;
    //food generation timer
    public float foodTime = 10f;
    private float currentFoodTime;
    //reference for the text UI element
    public Text statsText;
    //reference to the  
    NamelessPortal portal;
    private int cubeCountCurrent;
    private int enemyCountCurrent;
    
    // Start is called before the first frame update
    public void Start()
    {
        Button resetEvent = reset.GetComponent<Button>();
        resetEvent.onClick.AddListener(TaskOnClick);
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
        health = 20f;
        hunger = 20f;
        alive = true;
        LoadData();
        healthScore = System.Math.Round(health, 0);
    }
    // Update is called once per frame
    public void Update()
    {
        enemyCountMax = GameObject.FindGameObjectsWithTag("isEnemy").Length;
        //Shows the stats of Mr.Nameless Cube on screen
        statsText.text = "Health: " + healthScore.ToString() + " Hunger: " + hunger.ToString();
        //Kills Mr.Nameless Cube if he falls into the void
        if (mrcube.position.y < 0)
        {
            health = 0;
        }
        //tests if Mr.Nameless cube is still alive
        if (health >= 1)
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
            dps = dps * Time.deltaTime;
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
            Instantiate(enemy, new Vector3(Random.Range(-1000, 1000), 2, Random.Range(-1000, 1000)), Quaternion.identity);
            PlayerPrefs.Save();
        }
        newEnemyCountMax = enemyCountMax + 1;
        if (GameObject.FindGameObjectWithTag("enemyInit") != null) {
            GameObject.FindGameObjectWithTag("enemyInit").name = "enemy" + newEnemyCountMax.ToString();
            GameObject.FindGameObjectWithTag("enemyInit").tag = "isEnemy";
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
        if (collisionInfo.collider.tag == "canResetJump" || collisionInfo.collider.tag == "isCube")
        {
            //sets jumpReset bool to true
            jumpReset = true;
        }
        if (collisionInfo.collider.tag == "isFood")
        {
            if (hunger <= 20)
            {
                Destroy(collisionInfo.gameObject);
            }
            //determines what to do with food
            if (hunger <= 20f && health < 20f)
            {
                health++;
            }
            else
            {
                hunger = hunger + maxHunger - hunger;
            }

          
        }
    }
    void SaveData()
    {
        cubeCountMax = GameObject.FindGameObjectsWithTag("isCube").Length;
        PlayerPrefs.SetInt("cube-count-max", cubeCountMax);
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("isCube");
        cubeCountCurrent = 1;
        while (cubeCountCurrent <= cubeCountMax)
        {
            PlayerPrefs.SetFloat("cube" + cubeCountCurrent.ToString() + "-position-x", cubes[cubeCountCurrent].transform.position.x);
            PlayerPrefs.SetFloat("cube" + cubeCountCurrent.ToString() + "-position-y", cubes[cubeCountCurrent].transform.position.y);
            PlayerPrefs.SetFloat("cube" + cubeCountCurrent.ToString() + "-position-z", cubes[cubeCountCurrent].transform.position.z);
            cubeCountCurrent++;
        }
        PlayerPrefs.SetInt("enemy-count-max", enemyCountMax);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("isEnemy");
        enemyCountCurrent = 1;
        while (enemyCountCurrent <= enemyCountMax)
        {
            PlayerPrefs.SetFloat("enemy" + enemyCountCurrent.ToString() + "-position-x", enemies[enemyCountCurrent].transform.position.x);
            PlayerPrefs.SetFloat("enemy" + enemyCountCurrent.ToString() + "-position-y", enemies[enemyCountCurrent].transform.position.y);
            PlayerPrefs.SetFloat("enemy" + enemyCountCurrent.ToString() + "-position-z", enemies[enemyCountCurrent].transform.position.z);
            enemyCountCurrent++;
        }
        //Saves all the data
        PlayerPrefs.SetFloat("mrcube-pos-x", mrcube.position.x);
        PlayerPrefs.SetFloat("mrcube-pos-y", mrcube.position.y);
        PlayerPrefs.SetFloat("mrcube-pos-z", mrcube.position.z);
        PlayerPrefs.SetFloat("mrcube-health", health);
        PlayerPrefs.SetFloat("mrcube-hunger", hunger);
        PlayerPrefs.SetFloat("mrcube-spawn-x", spawnPoint.x);
        PlayerPrefs.SetFloat("mrcube-spawn-y", spawnPoint.y);
        PlayerPrefs.SetFloat("mrcube-spawn-z", spawnPoint.z);
        PlayerPrefs.SetFloat("mrcube-weight", weight);
        PlayerPrefs.SetFloat("planet-gravity", gravity);
        PlayerPrefs.SetFloat("mrcube-y-force", yForce);
        PlayerPrefs.SetFloat("mrcube-max-jump", maxJump);
        PlayerPrefs.SetString("mrcube-leftkey", leftKey);
        PlayerPrefs.SetString("mrcube-rightkey", rightKey);
        PlayerPrefs.SetString("mrcube-forwardkey", forwardKey);
        PlayerPrefs.SetString("mrcube-backwardkey", backwardKey);
        PlayerPrefs.SetString("mrcube-jumpkey", jumpKey);
    }

    void LoadData()
    {
        cubeCountCurrent = 1;
        cubeCountMax = PlayerPrefs.GetInt("cube-count-max");
        while (cubeCountCurrent <= cubeCountMax)
        {
            Instantiate(cube, new Vector3(PlayerPrefs.GetFloat("cube" + cubeCountCurrent.ToString() + "-position-x"), PlayerPrefs.GetFloat("cube" + cubeCountCurrent.ToString() + "-position-y"), PlayerPrefs.GetFloat("cube" + cubeCountCurrent.ToString() + "-position-z")), Quaternion.identity);
            cubeCountCurrent++;
        }
        enemyCountCurrent = 1;
        enemyCountMax = PlayerPrefs.GetInt("enemy-count-max");
        while (enemyCountCurrent <= enemyCountMax)
        {
            Instantiate(enemy, new Vector3(PlayerPrefs.GetFloat("enemy" + enemyCountCurrent.ToString() + "-position-x"), PlayerPrefs.GetFloat("enemy" + enemyCountCurrent.ToString() + "-position-y"), PlayerPrefs.GetFloat("enemy" + enemyCountCurrent.ToString() + "-position-z")), Quaternion.identity);
            GameObject.FindGameObjectWithTag("enemyInit").name = "enemy" + enemyCountMax + 1.ToString();
            enemyCountCurrent++;
        }
        //Loads all the data
        Vector3 getPos = new Vector3(PlayerPrefs.GetFloat("mrcube-pos-x", 0), PlayerPrefs.GetFloat("mrcube-pos-y", 1), PlayerPrefs.GetFloat("mrcube-pos-z", 0));
        transform.position = getPos;
        health = PlayerPrefs.GetFloat("mrcube-health", 20);
        hunger = PlayerPrefs.GetFloat("mrcube-hunger", 20);
        Vector3 getSpawn = new Vector3(PlayerPrefs.GetFloat("mrcube-spawn-x", 0), PlayerPrefs.GetFloat("mrcube-spawn-y", 1), PlayerPrefs.GetFloat("mrcube-spawn-z", 0));
        spawnPoint = getSpawn;
        weight = PlayerPrefs.GetFloat("mrcube-weight", 1);
        gravity = PlayerPrefs.GetFloat("planet-gravity", 1);
        leftKey = PlayerPrefs.GetString("mrcube-leftkey", "a");
        rightKey = PlayerPrefs.GetString("mrcube-rightkey", "d");
        forwardKey = PlayerPrefs.GetString("mrcube-forwardkey", "w");
        backwardKey = PlayerPrefs.GetString("mrcube-backwardkey", "s");
        jumpKey = PlayerPrefs.GetString("mrcube-jumpkey", "space");
        yForce = PlayerPrefs.GetFloat("mrcube-y-force", 1350);
        maxJump = PlayerPrefs.GetFloat("mrcube-max-jump");

    }
    void TaskOnClick()
    {
        SceneManager.LoadScene("Logan's Namless Game");
        PlayerPrefs.DeleteAll();
    }
    void OnApplicationQuit()
    {
        SaveData();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "isEnemy")
        {
            health = health - dps;
            healthScore = System.Math.Round(health, 0);
            if (Input.GetKeyDown(attackKey))
            {
                PlayerPrefs.SetFloat(collision.collider.name + "health", PlayerPrefs.GetFloat(collision.collider.name + "health", 20) - attackDamage);
                if (PlayerPrefs.GetFloat(collision.collider.name + "health", 20) <= 0)
                {
                    Destroy(collision.collider.gameObject);
                }
            }
        }

    }
}