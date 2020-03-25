using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NamelessEnemy : MonoBehaviour
{
    private bool computerCheck;
    public Transform mrcube;
    NavMeshAgent enemy;
    // Start is called before the first frame update
    void Start()
    {
        mrcube = GameObject.FindGameObjectWithTag("isMrCube").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        enemy.destination = mrcube.position;
    }
   
}
