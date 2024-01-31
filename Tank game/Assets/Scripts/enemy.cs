using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public BaseVar basic = new BaseVar();
    public ScanVar scan = new ScanVar();
    public CanVar cann = new CanVar();
    public ProjectileVar proj = new ProjectileVar();
    public targetVar target = new targetVar();


    //holds the enemies base variables
    [Serializable]
    public class BaseVar
    {
        public int health;
        public int enemyType = 0;
        public int detect = 0;
        public bool groEne = false;
        public bool flyEne = false;
        public GameObject level;
        public GameObject player;
        public LayerMask pla;
    }

    //holds the enemies scan variables
    [Serializable]
    public class ScanVar
    {
    
    }

     //holds the enemies base variables
    [Serializable]
    public class CanVar
    {
        public float range;
        public float reload;
        public float delay;
        public float lockOn;
        public Transform turret;
        public ParticleSystem fired;
    }

    //holds the enemies base variables
    [Serializable]
    public class ProjectileVar
    {
        public float speed;
        public float range;
        public float lifeTime;
        public int damage;
        public GameObject bullet;
        public ParticleSystem explode;
    }

    //holds the enemies base variables
    [Serializable]
    public class targetVar
    {
        public bool spotted = false;
        public Transform tarEnd;
        public LineRenderer line;
    }

    private int locat;
    private bool dest; 
    private Transform[] pat;
    private ParticleSystem firing;
    public Vector3 plaPos;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        //allows the enemy to be spawned in then given the player and map
        basic.level = GameObject.FindGameObjectWithTag("Map");
        basic.player = GameObject.FindGameObjectWithTag("Player");

        //grabs the maps patrol points
        pat = basic.level.GetComponent<levelCon>().patrolPoint;
        agent = GetComponent<NavMeshAgent>();

        if (basic.enemyType == 0)
        {
            target.line = target.tarEnd.GetComponentInChildren<LineRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //scans for player
        scanning();

        //moves ground enemies
        if (basic.groEne == true)
        {
            groundTroop();
        }

        //aims and fires cannons
        if (basic.enemyType == 0)
        {
            cannonAtt();
        }
    }

    //will allow the enemy to detect player
    void scanning()
    {

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

    //determines how the enemies cannons can attack
    void cannonAtt()
    {
        if (basic.detect == 0)
        {
            //spins the cannon a circle
            cann.turret.Rotate(0, cann.lockOn * Time.deltaTime, 0, Space.Self);
        }
        else if (basic.detect == 1)
        {
            //straightens the cannon to face forward
            cann.turret.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else if (basic.detect == 2)
        {
            //locks the cannon on to the player
            Quaternion angle = Quaternion.LookRotation(basic.player.transform.position - cann.turret.position);
            Quaternion _LookAt = Quaternion.RotateTowards(cann.turret.rotation, angle, cann.lockOn);
            cann.turret.rotation = _LookAt;

            //checks if the cannnon is pointing directly at the player
            RaycastHit cannHit;
            if (Physics.Raycast(target.tarEnd.position, target.tarEnd.forward, out cannHit, cann.range, basic.pla))
            {
                if (cannHit.collider.transform == basic.player.transform)
                {
                    //countdown until the cannon fires
                    cann.reload -= 1 * Time.deltaTime;
                    target.spotted = true;
                }
                else
                {
                    cann.reload = cann.delay;
                    target.spotted = false;
                }
            }
            else
            {
                target.spotted = false;
            }

            //fires the cannon
            if (cann.reload <= 0)
            {
                firing = Instantiate(cann.fired);
                firing.transform.position = target.tarEnd.position;
                firBull();
                Destroy(firing);
                cann.reload = cann.delay;
            }

            targeting(cann.reload, cann.delay);
        }
    }

    void firBull()
    {

    }
    void targeting(float tar, float max)
    {
        if (target.spotted == true)
        {
            float check = tar / max;
            var grad = Color.Lerp(Color.green, Color.red, check);
            target.line.positionCount = 2;
            target.line.SetPosition(0, target.tarEnd.position);
            target.line.SetPosition(1, basic.player.transform.position);
            target.line.material.SetColor("_Color", grad);
        }
        else
        {
            target.line.positionCount = 0;
            target.line.material.SetColor("_Color", Color.red);
        }
    }

    public void takeDamage(int dam)
    {
        basic.health -= dam;

        if (basic.health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
