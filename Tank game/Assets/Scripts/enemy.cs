using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public BaseVar basic = new BaseVar();
    public ScanVar scan = new ScanVar();
    public AniVar ani = new AniVar();

    //holds the enemies base variables
    [Serializable]
    public class BaseVar
    {
        public float gunRot = 0;
        public int health;
        public int enemyType = 0;
        public int detect = 0;
        public int gunType = 0;
        public Transform[] gun;
        public Transform[] turConnect;
        public GameObject level;
        public GameObject player;
        public Vector3 gunSize;
    }

    //holds the enemies scan variables
    [Serializable]
    public class ScanVar
    {
        public float rad;
        public float ang;
        public float searching;
        public float searchTime;
        public bool spotted;
    }

    //holds the enemies ani variables
    [Serializable]
    public class AniVar
    {
        public Transform model;
        public Animator anim;
    }

    private int locat;
    private bool dest; 
    private Transform[] pat;
    public Vector3 plaPos;
    private NavMeshAgent agent;

    public bool InFOV()
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(transform.position, scan.rad, overlaps);

        for (int i = 0; i < count + 1; i++)
        {

            Vector3 directionBetween = (basic.player.transform.position - transform.position).normalized;
            directionBetween.y *= 0;

            float angle = Vector3.Angle(transform.forward, directionBetween);

            if (angle <= scan.ang)
            {
                Ray ray;              
                ray = new Ray(transform.position, basic.player.transform.position - transform.position);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, scan.rad))
                {
                    if (hit.transform == basic.player.transform)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //allows the enemy to be spawned in then given the player and map
        basic.level = GameObject.FindGameObjectWithTag("Map");
        basic.player = GameObject.FindGameObjectWithTag("Player");

        //grabs the maps patrol points
        pat = basic.level.GetComponent<levelCon>().patrolPoint;
        agent = GetComponent<NavMeshAgent>();

        basic.gun = new Transform[basic.turConnect.Length];

        for(int i = 0; i < basic.turConnect.Length; i++)
        { 
            basic.gunType = UnityEngine.Random.Range(0, 1);
            basic.gun[i] = Instantiate(basic.level.GetComponent<levelCon>().enemyGuns[basic.gunType].transform, basic.turConnect[i].position, basic.turConnect[i].rotation);
            basic.gun[i].localScale = basic.gunSize;
            basic.gun[i].parent = basic.turConnect[i].parent;
            basic.gun[i].GetComponent<enemyGuns>().basic.rota = basic.gunRot;
            basic.gun[i].GetComponent<enemyGuns>().basic.enemy = gameObject.transform;

            if (basic.enemyType == 0 || basic.enemyType == 1 || basic.enemyType == 2)
            {
                basic.gun[i].GetComponent<enemyGuns>().basic.spin = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //scans for player
        scanning();

        //runs the tank enemies enemies
        if (basic.enemyType == 0 || basic.enemyType == 1 || basic.enemyType == 2 || basic.enemyType == 3 || basic.enemyType == 4)
        {
            groundTroop();
        }


        if (basic.enemyType == 2)
        {
            animate();
        }
    }

    //will allow the enemy to detect player
    void scanning()
    {
         scan.spotted = InFOV();

        if (scan.searching <= 1 && basic.detect == 1)
        {            
            basic.detect = 0;
            scan.searching = -1;
        }

        if (basic.detect == 2 && scan.spotted == false)
        {
            basic.detect = 2;
            scan.searching -= 1 * Time.deltaTime;

            if (scan.searching <= 1)
            {
                basic.detect = 1;
                scan.searching = scan.searchTime;
            }
        }
        else if (basic.detect == 1 && scan.spotted == false)
        {            
            basic.detect = 1;
            scan.searching -= 1 * Time.deltaTime;
        }

        if (scan.spotted == true)
        {
            basic.detect = 2;
            scan.searching = scan.searchTime;
        }
    }

    //determines how grounded troop (enemy tanks) move
    void groundTroop()
    {
        if (basic.detect == 0)
        {
            //picks a patrol point to move towards
            if (dest == false)
            {
                locat = UnityEngine.Random.Range(0, pat.Length - 1);
                dest = true;
            }
            
            //moves toward the patrol point
            agent.destination = pat[locat].position;

            //detects that the enemy has reached the patrol point
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                dest = false;
            }
        }
        else if (basic.detect == 1)
        {
            //locks on to the players last known position
            agent.destination = plaPos;
        }
         else if (basic.detect == 2)
        {
            //moves towards the player and records their position
            agent.destination = basic.player.transform.position;
            plaPos = basic.player.transform.position;
        }
    }

    void animate()
    {
        ani.model.Rotate(0, 1, 0, Space.Self);
    }

    public void takeDamage(int dam)
    {
        basic.health -= dam;

        if (basic.enemyType == 4 && dam >= 10)
        {
            ani.anim.SetTrigger("hit");
        }

        if (basic.health <= 0)
        {           
            if (basic.enemyType == 0 || basic.enemyType == 1 || basic.enemyType == 2)
            {
                Destroy(gameObject);
            }
            else
            {
                ani.anim.SetBool("dead", true);
                Destroy(gameObject, 0.75f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scan.rad);

        Vector3 fovLine1 = Quaternion.AngleAxis(scan.ang, transform.up) * transform.forward * scan.rad;
        Vector3 fovLine2 = Quaternion.AngleAxis(-scan.ang, transform.up) * transform.forward * scan.rad;

        Vector3 fovLine3 = Quaternion.AngleAxis(360, -transform.up) * -transform.up * 15;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
        Gizmos.DrawRay(transform.position, fovLine3);

        if (!scan.spotted)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * scan.rad);
    }
}
