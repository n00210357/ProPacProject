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
        public float xSen, ySen;
        public float total;
    }

    //holds the main cannon variables
    [Serializable]
    public class MainCannon
    {
        public float range;
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

    }

    void Start()
    {
        //draws a line at what the tank is aiming at
        main.aimAssets = main.canExit.GetComponent<LineRenderer>();
        main.aimAssets.positionCount = 2;

        // Hides the cursor
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        camCon();
        mainTurret();
    }

    //controls the rotation of the turret
    void camCon()
    {
        aim.yAxis += -Input.GetAxis("Mouse X") * aim.xSen * -1;
        transform.eulerAngles = new Vector3(0, aim.yAxis, 0);

        aim.xAxis += -Input.GetAxis("Mouse Y") * aim.ySen * 1;

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
            if (Input.GetKeyDown(saveData.keybindings.keys[0]))
            {
                ParticleSystem firing = Instantiate(main.fire);
                ParticleSystem blast = Instantiate(main.explosion);
                firing.transform.position = main.canExit.position;
                blast.transform.position = hit.point;
                Destroy(firing, 0.5f);
                Destroy(blast, 0.5f);
            }
        }

        Debug.DrawRay(main.canExit.position, main.canExit.forward * main.range, Color.red);
    }
}
