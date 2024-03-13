using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guns : MonoBehaviour
{
    public AimVar aim = new AimVar();
    public MainCannon main = new MainCannon();
    public SecondaryCannon sec = new SecondaryCannon();

     //holds the aiming variables
    [Serializable]
    public class AimVar
    {
        public float xAxis, yAxis;
        public float pTotal;
        public float mTotal;
        public float range;
        public bool recenter;
        public LayerMask lockOn;
    }

    //holds the main cannon variables
    [Serializable]
    public class MainCannon
    {
        public float delay;
        public float reload;
        public float reloadSpeed;
        public float radius;
        public int damage;
        public int ammo;
        public int maxAmmo;
        public Transform canHold;
        public Transform canExit;
        public Transform canFirst;
        public Transform[] cannons;
        public ParticleSystem fire;
        public ParticleSystem smaBla;
        public ParticleSystem bigBla;
        public LineRenderer aimAssets;
    }

    //holds the side arms variables
    [Serializable]
    public class SecondaryCannon
    {
        public int fireSpeed;
        public int reloadSpeed;
        public int ammo;
        public int maxAmmo;
        public bool reloading;
        public bool firing;
        public Transform[] guns;
        public GameObject hole;
    }

    private GameObject firing;
    private GameObject blast;
    private Vector3 positioning;
    private ParticleSystem explosion;
    private RaycastHit hit;

    void Start()
    {
        positioning = main.canFirst.localPosition;

        //draws a line at what the tank is aiming at
        main.aimAssets = main.canExit.GetComponent<LineRenderer>();
        main.aimAssets.positionCount = 2;
    }

    void Update()
    {        
        camCon();
        gunAim();
        mainTurret();
        secTurret();
    }

    void FixedUpdate()
    {
        upgrades();
    }

    //controls the rotation of the turret
    void camCon()
    {
        if (Input.GetKeyDown(saveData.keybindings.keys[2]))
        {
            aim.recenter = true;
        }

        if (transform.parent.transform.parent.GetComponent<player>().basic.vehicalType == true || aim.recenter == true)
        {
            if (aim.xAxis >= -0.5 && aim.xAxis <= 0.5)
            {
                aim.xAxis = 0;
            }
            else if (aim.xAxis > 0)
            {
                aim.xAxis -= 1;
            }
            else if (aim.xAxis < 0)
            {
                aim.xAxis += 1;
            }

            if (aim.yAxis >= -0.5 && aim.yAxis <= 0.5)
            {
                aim.yAxis = 0;
            }
            else if (aim.yAxis > 0)
            {
                aim.yAxis -= 1;
            }
            else if (aim.yAxis < 0)
            {
                aim.yAxis += 1;
            }

            if (aim.yAxis == 0 && aim.xAxis == 0)
            {
                aim.recenter = false;
            }
        }
        else
        {
            aim.yAxis += -Input.GetAxis("Mouse X") * saveData.keybindings.cam[2] * -1;
            aim.xAxis += -Input.GetAxis("Mouse Y") * saveData.keybindings.cam[3] * 1;

            if (aim.xAxis >= aim.pTotal)
            {
                aim.xAxis = aim.pTotal;
            }
            else if (aim.xAxis <= aim.mTotal)
            {
                aim.xAxis = aim.mTotal;
            }
        }

        transform.localEulerAngles = new Vector3(0, aim.yAxis, 0);
        main.canHold.localRotation = Quaternion.Euler(aim.xAxis, 0, 0);

        if (aim.yAxis <= -361)
        {
            aim.yAxis = 359;
        }
        else if (aim.yAxis >= 361)
        {
            aim.yAxis = -359;
        }
    }

    void gunAim()
    {
        if (Physics.Raycast(main.canExit.position, main.canExit.forward, out hit, aim.range, aim.lockOn))
        {
            hit.point = hit.point;
        }
    }

    //allows the player to control the main cannon
    void mainTurret()
    {
        main.aimAssets.SetPosition(0, main.canExit.position);
        main.aimAssets.SetPosition(1, hit.point);

        //fires main cannon
        if (Input.GetKeyDown(saveData.keybindings.keys[0]) && main.ammo >= 1)
        {
            firing = Instantiate(main.fire.gameObject);
            blast = Instantiate(explosion.gameObject);
            firing.transform.position = main.canExit.position;
            blast.transform.position = hit.point;
            main.ammo -= 1;

            Collider[] hitEnemies = Physics.OverlapSphere(hit.point, main.radius);
            foreach (Collider tar in hitEnemies)
            {
                if (tar.GetComponent<enemy>())
                {
                    tar.GetComponent<enemy>().takeDamage(main.damage);
                }
            }
        }        

        if (main.ammo <= main.maxAmmo - 1)
        {
            main.reload += main.reloadSpeed * Time.deltaTime;
        }

        if (main.reload >= main.delay)
        {
            main.reload = 0;
            main.ammo += 1;
        }

        Destroy(firing, 2f);
        Destroy(blast, 2f);
    }

    void secTurret()
    {
        sec.guns[0].LookAt(hit.point);
        sec.guns[1].LookAt(hit.point);

        //fires secondary cannon
        if (Input.GetKey(saveData.keybindings.keys[1]) && sec.ammo >= 1 && sec.reloading == false)
        {
            foreach (Transform gu in sec.guns)
            {
                gu.GetChild(2).GetComponent<ParticleSystem>().Play();

                if (gu.localPosition.z >= 0.25f)
                {
                    sec.firing = true;
                }                
                
                if (gu.localPosition.z <= -0.25f)
                {
                    sec.firing = false;
                }                

                if (sec.firing == true)
                {
                    gu.localPosition = new Vector3(gu.localPosition.x, gu.localPosition.y, gu.localPosition.z - 5 * Time.deltaTime);
                }
                else
                {
                    gu.localPosition = new Vector3(gu.localPosition.x, gu.localPosition.y, gu.localPosition.z + 5 * Time.deltaTime);
                }
            }
                        
            sec.ammo -= sec.fireSpeed;

            if (hit.transform.gameObject.layer == 7)
            { 
                if (hit.transform.GetComponent<enemy>())
                {
                    hit.transform.GetComponent<enemy>().takeDamage(sec.fireSpeed);
                }
            }

            if (hit.transform.gameObject.layer == 0)
            {
                GameObject newHole = Instantiate(sec.hole, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                //newHole.transform.LookAt(hit.point + hit.normal * 0.001f);
                newHole.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
                newHole.transform.parent = hit.transform;
                Destroy(newHole, 5f);
            }
        }
 
        if (sec.ammo <= 0 || Input.GetKeyDown(saveData.keybindings.keys[7]))
        {
            sec.reloading = true;
        }
        
        if (sec.reloading == true && sec.ammo <= sec.maxAmmo)
        {
            sec.ammo += sec.reloadSpeed;
        }
        else if (sec.reloading == true)
        {
            sec.ammo = sec.maxAmmo;
            sec.reloading = false;
        }
    }

    void upgrades()
    {
        //cannon upgrades
        saveData.upgrades.cannShots[0] = true;

        for (int i = 0; i < 6; i++)
        {    
            main.cannons[i].GetComponentInChildren<MeshRenderer>().enabled = saveData.upgrades.cannShots[i];
                
            if ((main.maxAmmo == 1 || main.maxAmmo == 3 || (main.maxAmmo == 5)) && i == (main.maxAmmo - 1))
            {
                main.cannons[i].localPosition = new Vector3(0f, main.cannons[i].localPosition.y, main.cannons[i].localPosition.z);
            }
            else if (i == 0 || i == 2 || i == 4)
            {
                main.cannons[i].localPosition = new Vector3(0.2f, main.cannons[i].localPosition.y, main.cannons[i].localPosition.z);
            }
            else
            {
                main.cannons[i].localPosition = new Vector3(-0.2f, main.cannons[i].localPosition.y, main.cannons[i].localPosition.z);
            }

            if (i <= main.ammo - 1)
            {
                main.cannons[i].localPosition = new Vector3(main.cannons[i].localPosition.x, main.cannons[i].localPosition.y, 1.65f);
            }
            else
            {
                main.cannons[i].localPosition = new Vector3(main.cannons[i].localPosition.x, main.cannons[i].localPosition.y, 0.48f);
            }
        }        

        if (main.maxAmmo < saveData.upgrades.cannShots.Length)
        {
            if (saveData.upgrades.cannShots[main.maxAmmo - 1] == true)
            {                
                main.maxAmmo += 1;
            }
        }

        if (saveData.upgrades.cannShots[main.maxAmmo - 1] == false)
        {
            main.maxAmmo -= 1;
        }

        if (main.ammo > main.maxAmmo)
        {
            main.ammo -= 1;
        }

        if (saveData.upgrades.canReloadSpeed == true)
        {
            main.reloadSpeed = 2;
        }
        else
        {
            main.reloadSpeed = 1;
        }

        if (saveData.upgrades.shotRadius == true)
        {
            main.radius = 20;
            explosion = main.bigBla;
        }
        else
        {
            main.radius = 10;
            explosion = main.smaBla;
        }

        //mach upgrades
        if (sec.fireSpeed < saveData.upgrades.machAmmo.Length - 1)
        {
            if (saveData.upgrades.machSpeed[sec.fireSpeed] == true)
            {
                sec.fireSpeed += 1;
            }
        }
                
        for (int i = 1; i < saveData.upgrades.machSpeed.Length; i++)
        {
            if (saveData.upgrades.machSpeed[i] == false && i == 1)
            {
                sec.guns[0].GetChild(0).localPosition = new Vector3(0.73f, sec.guns[0].GetChild(0).localPosition.y, sec.guns[0].GetChild(0).localPosition.z);
                sec.guns[0].GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                sec.guns[1].parent.gameObject.SetActive(false);
                break;
            }

            if (saveData.upgrades.machSpeed[i] == false && i == 2)
            {
                sec.guns[0].GetChild(0).localPosition = new Vector3(0.73f, sec.guns[0].GetChild(0).localPosition.y, sec.guns[0].GetChild(0).localPosition.z);
                sec.guns[0].GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                sec.guns[1].parent.gameObject.SetActive(true);
                sec.guns[1].GetChild(0).localPosition = new Vector3(-0.77f, sec.guns[0].GetChild(0).localPosition.y, sec.guns[0].GetChild(0).localPosition.z);
                sec.guns[1].GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                break;
            }

            if (saveData.upgrades.machSpeed[i] == true && i == 2)
            {
                sec.guns[0].GetChild(0).localPosition = new Vector3(0.63f, sec.guns[0].GetChild(0).localPosition.y, sec.guns[0].GetChild(0).localPosition.z);
                sec.guns[0].GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                sec.guns[1].parent.gameObject.SetActive(true);
                sec.guns[1].GetChild(0).localPosition = new Vector3(-0.67f, sec.guns[0].GetChild(0).localPosition.y, sec.guns[0].GetChild(0).localPosition.z);
                sec.guns[1].GetChild(1).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                break;
            }
        }

        if (saveData.upgrades.machSpeed[sec.fireSpeed - 1] == false)
        {
            sec.fireSpeed -= 1;
        }
        
        if (((sec.maxAmmo / 500) - 1) < saveData.upgrades.machAmmo.Length)
        {
            if (((sec.maxAmmo / 500) - 1) <= 2)
            {
                if (saveData.upgrades.machAmmo[sec.maxAmmo / 500] == true)
                {
                    sec.maxAmmo += 500;
                }
            }
        }
 
        if (saveData.upgrades.machAmmo[(sec.maxAmmo / 500) - 1] == false)
        {
            sec.maxAmmo -= 500;
        }

        if (sec.ammo > sec.maxAmmo)
        {
            sec.ammo -= 500;
        }

        if (saveData.upgrades.machReloadSpeed == true)
        {
            sec.reloadSpeed = 2;
        }
        else
        {
            sec.reloadSpeed = 1;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hit.point, main.radius);
    }
}
