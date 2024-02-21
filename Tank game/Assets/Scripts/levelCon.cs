using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class levelCon : MonoBehaviour
{
    public NavMeshSurface sur;

    //the enemy spawn points 
    public Transform[] spawnPoint;

    //the maps patrol points
    public Transform[] patrolPoint;

    //what enemies can be spawned
    public GameObject[] enemiesToSpawn;

    //enemies choice of gun
    public GameObject[] enemyGuns;

    //detect what enemies are alive
    public GameObject[] spawnedEnemies;

    void Start()
    {
        sur.BuildNavMesh();

        if (spawnPoint[0] == null)
        {
            spawnPoint = new Transform[patrolPoint.Length];

            for (int i = 0; i <= patrolPoint.Length - 1; i++)
            {
                spawnPoint[i] = patrolPoint[i];
            }
        }


    }
}
