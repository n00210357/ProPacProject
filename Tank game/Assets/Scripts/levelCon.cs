using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class levelCon : MonoBehaviour
{
    public float time;
    public float low;
    public float hig;

    //level type 0 = arena  1 = survival  2 = race  3 = randomizes between arena or survival
    public int type;
    public bool activate;
    public NavMeshSurface sur;
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
        //selects level type
        if (type == 3)
        {
            type = Random.Range(0, 2);            
        }

        if (type == 1 || type == 2)
        {
            time = Random.Range(low, hig);
        }

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

        if (type != 2)
        {
            spawnEnemies();
        }
    }

    //spawns tunnels
    void spawnTunnels()
    {
        foreach(Transform con in connectionPoints)
        {
            GameObject tun = Instantiate(tunnel, con.position, con.rotation);
            tun.transform.parent = transform;
        }
    }

    //spawns enemies
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
                spawnedEnemies[i].transform.parent = transform;
            }
        }
    }

    void Update()
    {
        //the effects of each level type
        if (type == 1)
        {
            for (int i = 0; i <= spawnedEnemies.Length - 1; i++)
            {
                if (spawnedEnemies[i] == null)
                {
                    int ra = Random.Range(0, 10000);
                    
                    if (ra == 0)
                    {
                        int pos = Random.Range(0, spawnPoint.Length);
                        int type = Random.Range(0, enemiesToSpawn.Length);
                        spawnedEnemies[i] = Instantiate(enemiesToSpawn[type], spawnPoint[pos].position, spawnPoint[pos].rotation);
                        spawnedEnemies[i].transform.parent = transform;
                    }
                }                
            }
        }
        else if (type == 2)
        {
            if (time <= 0)
            {
                for (int i = 0; i <= spawnedEnemies.Length - 1; i++)
                {
                    if (spawnedEnemies[i] == null)
                    {
                        int ra = Random.Range(0, 10000);

                        if (ra == 0)
                        {
                            int pos = Random.Range(0, spawnPoint.Length);
                            int type = Random.Range(0, enemiesToSpawn.Length);
                            spawnedEnemies[i] = Instantiate(enemiesToSpawn[type], spawnPoint[pos].position, spawnPoint[pos].rotation);
                            spawnedEnemies[i].transform.parent = transform;
                        }
                    }
                }

                time = Random.Range(low, hig);                
            }
        }

        if (time >= 0 && activate == true)
        {
            time -= 1 * Time.deltaTime;            
        }
    }

    //detects player
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<player>())
        {
            activate = true;
        }
    }
}