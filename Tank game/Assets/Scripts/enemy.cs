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
        public float rad;
        public float ang;
        public float searching;
        public float searchTime;
        public bool spotted;
        public Transform tarEnd;
        public LineRenderer line;
    }

     //holds the enemies cannon variables
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

    //holds the enemies projectile variables
    [Serializable]
    public class ProjectileVar
    {
        public float speed;
        public float range;
        public float lifeTime;
        public int damage;
        public int type;
        public bool killable;
        public GameObject bullet;
        public LayerMask bul;
        public LayerMask tar;
        public ParticleSystem explode;
    }

    private int locat;
    private bool dest; 
    private Transform[] pat;
    private ParticleSystem firing;
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

            float angle = Vector3.Angle(cann.turret.forward, directionBetween);

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

        if (basic.enemyType == 0)
        {
            scan.line = scan.tarEnd.GetComponentInChildren<LineRenderer>();
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
         scan.spotted = InFOV();

        if (scan.searching <= 1 && basic.detect == 1)
        {
            basic.detect = 0;
            scan.searching = -1;
        }

        if (scan.spotted == true)
        {
            basic.detect = 2;
            scan.searching = scan.searchTime;
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
            if (Physics.Raycast(scan.tarEnd.position, scan.tarEnd.forward, out cannHit, cann.range, basic.pla))
            {
                if (cannHit.collider.transform == basic.player.transform)
                {
                    //countdown until the cannon fires
                    cann.reload -= 1 * Time.deltaTime;
                    scan.spotted = true;
                }
                else
                {
                    cann.reload = cann.delay;
                    scan.spotted = false;
                }
            }
            else
            {
                scan.spotted = false;
            }

            //fires the cannon
            if (cann.reload <= 0)
            {
                firing = Instantiate(cann.fired);
                firing.transform.position = scan.tarEnd.position;
                firBull();
                Destroy(firing);
                cann.reload = cann.delay;
            }

            targeting(cann.reload, cann.delay);
        }
    }

    void firBull()
    {
        GameObject projectile = Instantiate(proj.bullet, scan.tarEnd.position, scan.tarEnd.rotation);
        projectile.layer = gameObject.layer;
        projectile.GetComponent<bullet>().speed = proj.speed;
        projectile.GetComponent<bullet>().range = proj.range;
        projectile.GetComponent<bullet>().lifeTime = proj.lifeTime;
        projectile.GetComponent<bullet>().damage = proj.damage;
        projectile.GetComponent<bullet>().type = proj.type;
        projectile.GetComponent<bullet>().killable = proj.killable;
        projectile.GetComponent<bullet>().bullLayer = proj.bul;
        projectile.GetComponent<bullet>().tarLayer = proj.tar;
        projectile.GetComponent<bullet>().explode = proj.explode;
    }

    void targeting(float tar, float max)
    {
        if (scan.spotted == true)
        {
            float check = tar / max;
            var grad = Color.Lerp(Color.green, Color.red, check);
            scan.line.positionCount = 2;
            scan.line.SetPosition(0, scan.tarEnd.position);
            scan.line.SetPosition(1, basic.player.transform.position);
            scan.line.material.SetColor("_Color", grad);
        }
        else
        {
            scan.line.positionCount = 0;
            scan.line.material.SetColor("_Color", Color.red);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, scan.rad);

        Vector3 fovLine1 = Quaternion.AngleAxis(scan.ang, cann.turret.up) * cann.turret.forward * scan.rad;
        Vector3 fovLine2 = Quaternion.AngleAxis(-scan.ang, cann.turret.up) * cann.turret.forward * scan.rad;

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
