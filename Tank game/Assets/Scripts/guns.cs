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
        public LayerMask lockOn;
    }

    //holds the main cannon variables
    [Serializable]
    public class MainCannon
    {
        public float delay;
        public float reload;
        public float radius;
        public int damage;
        public int ammo;
        public int maxAmmo;
        public Transform can;
        public Transform canExit;
        public ParticleSystem fire;
        public ParticleSystem explosion;
        public LineRenderer aimAssets;
    }

    //holds the side arms variables
    [Serializable]
    public class SecondaryCannon
    {
        public int ammo;
        public int maxAmmo;
        public bool reloading;
        public Transform[] guns;
    }

    public bool UI;
    private GameObject firing;
    private GameObject blast;
    private RaycastHit hit;

    void Start()
    {
        //draws a line at what the tank is aiming at
        main.aimAssets = main.canExit.GetComponent<LineRenderer>();
        main.aimAssets.positionCount = 2;
    }

    void Update()
    {
        camCon();
        gunAim();

        // Hides the cursor
        if (UI == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            mainTurret();
            secTurret();
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //controls the rotation of the turret
    void camCon()
    {
        if (transform.parent.GetComponent<player>().basic.vehicalType == false)
        {
            aim.yAxis += -Input.GetAxis("Mouse X") * saveData.keybindings.xSen * -1;
            aim.xAxis += -Input.GetAxis("Mouse Y") * saveData.keybindings.ySen * 1;

            if (aim.xAxis >= aim.pTotal)
            {
                aim.xAxis = aim.pTotal;
            }
            else if (aim.xAxis <= aim.mTotal)
            {
                aim.xAxis = aim.mTotal;
            }
        }
        else
        {
            if (aim.xAxis >= -0.5 && aim.xAxis <= 0.5)
            {
                aim.xAxis = 0;
            }
            else if (aim.xAxis > 0)
            {
                aim.xAxis -= 1;
            }
            else if(aim.xAxis < 0)
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
        }

        transform.localEulerAngles = new Vector3(0, aim.yAxis, 0);
        main.can.localRotation = Quaternion.Euler(aim.xAxis, 0, 0);
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
            blast = Instantiate(main.explosion.gameObject);
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
            main.reload += 1 * Time.deltaTime;
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
        //fires secondary cannon
        if (Input.GetKey(saveData.keybindings.keys[1]) && sec.ammo >= 1 && sec.reloading == false)
        {
            sec.ammo -= 1;

            Collider[] hitEnemies = Physics.OverlapSphere(hit.point, main.radius);
            foreach (Collider tar in hitEnemies)
            {
                if (tar.GetComponent<enemy>())
                {
                    tar.GetComponent<enemy>().takeDamage(1);
                }
            }
        }

        foreach(Transform gu in sec.guns)
        {
            gu.LookAt(hit.point);
        }
        

        if (sec.ammo <= 0 || Input.GetKeyDown(saveData.keybindings.keys[7]))
        {
            sec.reloading = true;
        }
        
        if (sec.reloading == true && sec.ammo <= sec.maxAmmo)
        {
            sec.ammo += 1;
        }
        else if (sec.reloading == true)
        {
            sec.ammo = sec.maxAmmo;
            sec.reloading = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hit.point, main.radius);
    }
}
