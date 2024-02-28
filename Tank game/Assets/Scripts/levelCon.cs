using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class levelCon : MonoBehaviour
{
    public NavMeshSurface sur;
    public Transform levCon;
    public GameObject tunnel;

    public Transform[] connectionPoints;

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

    private int spawnChance;
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

        spawnTunnels();
        spawnEnemies();
    }

    void spawnTunnels()
    {
        foreach(Transform con in connectionPoints)
        {
            Instantiate(tunnel, con.position, con.rotation);
        }
    }

    void spawnEnemies()
    {
        spawnedEnemies = new GameObject[spawnPoint.Length];

        for (int i = 0; i <= spawnPoint.Length - 1; i++)
        {
            spawnChance = Random.Range(0, 100);

            if (spawnChance <= saveData.difficulty)
            {
                int type = Random.Range(0, enemiesToSpawn.Length);
                spawnedEnemies[i] = Instantiate(enemiesToSpawn[type], spawnPoint[i].position, spawnPoint[i].rotation); 
            }
        }
    }
}
