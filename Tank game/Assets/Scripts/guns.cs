using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guns : MonoBehaviour
{
    public BaseVar basic = new BaseVar();
    public AimVar aim = new AimVar();
    public MainCannon main = new MainCannon();
    public SecondaryCannon sec = new SecondaryCannon();

    //holds the guns base variables
    [Serializable]
    public class BaseVar
    {
        public GameObject keyBindings;
    }

     //holds the aiming variables
    [Serializable]
    public class AimVar
    {
        public float xAxis, yAxis;
        public float total;
    }

    //holds the main cannon variables
    [Serializable]
    public class MainCannon
    {
        public float delay;
        public float reload;
        public float range;
        public float radius;
        public int damage;
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
    }

    public bool UI;

    void Start()
    {
        //draws a line at what the tank is aiming at
        main.aimAssets = main.canExit.GetComponent<LineRenderer>();
        main.aimAssets.positionCount = 2;

        // Hides the cursor
        if (UI == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        camCon();
        mainTurret();
        secTurret();
    }

    //controls the rotation of the turret
    void camCon()
    {
        aim.yAxis += -Input.GetAxis("Mouse X") * saveData.keybindings.xSen * -1;
        transform.eulerAngles = new Vector3(0, aim.yAxis, 0);

        aim.xAxis += -Input.GetAxis("Mouse Y") * saveData.keybindings.ySen * 1;

        if (aim.xAxis >= aim.total)
        {
            aim.xAxis = aim.total;
        }
        else if (aim.xAxis <= -aim.total)
        {
            aim.xAxis = -aim.total;
        }

        main.can.localRotation = Quaternion.Euler(aim.xAxis, 0, 0);
    }

    //allows the player to control the main cannon
    void mainTurret()
    {       
        main.aimAssets.SetPosition(0, main.canExit.position);

        //aims the cannnon
        RaycastHit hit;
        if (Physics.Raycast(main.canExit.position, main.canExit.forward, out hit, main.range))
        {
            main.aimAssets.SetPosition(1, hit.point);

            //fires main cannon
            if (Input.GetKeyDown(saveData.keybindings.keys[0]) && main.reload <= 0)
            {
                ParticleSystem firing = Instantiate(main.fire);
                ParticleSystem blast = Instantiate(main.explosion);
                firing.transform.position = main.canExit.position;
                blast.transform.position = hit.point;
                Destroy(firing, 0.5f);
                Destroy(blast, 0.5f);
                main.reload = main.delay;

                Collider[] hitEnemies = Physics.OverlapSphere(hit.point, main.radius);
                foreach (Collider tar in hitEnemies)
                {
                    if (tar.GetComponent<enemy>())
                    {
                        tar.GetComponent<enemy>().takeDamage(main.damage);
                    }
                }
            }
        }

        main.reload -= 1 * Time.deltaTime;
    }

    void secTurret()
    {
        //aims the cannnon
        RaycastHit hit;
        if (Physics.Raycast(main.canExit.position, main.canExit.forward, out hit, main.range))
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
}
