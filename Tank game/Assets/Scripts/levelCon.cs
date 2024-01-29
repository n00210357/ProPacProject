using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelCon : MonoBehaviour
{
    //the enemy spawn points 
    public Transform[] spawnPoint;

    //the maps patrol points
    public Transform[] patrolPoint;
    //detect what enemies are alive
    public GameObject[] spawnedEnemies;
    //what enemies can be spawned
    public GameObject[] enemiesToSpawn;
}
