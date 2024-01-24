using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guns : MonoBehaviour
{
    public AimVar aim = new AimVar();
    public MainCannon main = new MainCannon();
    public SecondaryCannon sec = new SecondaryCannon();

    [Serializable]
    public class AimVar
    {
        public float xAxis, yAxis;
        public float xSen, ySen;
        public float total;
    }

    [Serializable]
    public class MainCannon
    {
        public Transform can;
        public Transform canExit;
        public LineRenderer aimAssets;
    }

    [Serializable]
    public class SecondaryCannon
    {

    }

    void Start()
    {
        main.aimAssets = main.canExit.GetComponent<LineRenderer>();
    }

    void Update()
    {
        camCon();
        mainTurret();
    }

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

    void mainTurret()
    {
        main.aimAssets.positionCount = 2;
        main.aimAssets.SetPosition(0, main.canExit.position);
        main.aimAssets.SetPosition(1, main.canExit.position + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward + main.canExit.forward);

        //Raycasthit;
    }
}
