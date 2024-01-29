using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public BaseVar basic = new BaseVar();
    public DashVar dash = new DashVar();
    public TankVar tank = new TankVar();
    public CarVar car = new CarVar();

    //holds the players base variables
    [Serializable]
    public class BaseVar
    {
        public bool vehicalType = false;
        public GameObject keyBindings;
    }

    //hold the dash variables
    [Serializable]
    public class DashVar
    {
        public float dash;
        public float dashTimer;
        public float dashDelay;
        public int dashAmount;
        public int dashTotal;
    }

    //holds the tank modes variables
    [Serializable]
    public class TankVar
    {
        public float speed;
        public float turnSpeed;
        public float jump;
        public Vector3 movDir;
    }

    //holds the car modes variables
    [Serializable]
    public class CarVar
    {
        public float speed;
        public float reverse;
        public float turn;
        public float moveInput;
        public float turnInput;
        public Transform steeringWheel;
    }

    private Rigidbody tankRB;
    private Rigidbody carRB;

    void Start()
    {        
        //assigns the rigidbodies
        tankRB = GetComponent<Rigidbody>();
        carRB = car.steeringWheel.GetComponent<Rigidbody>();
    }

    void Update()
    {            
        //sets drive type to tank
        if (basic.vehicalType == false)
        {
            tankSteer();
            car.steeringWheel.parent = transform;

            //enables car mode
            if (Input.GetKeyDown(KeyCode.F))
            {
                basic.vehicalType = true;
                tankRB.isKinematic = true;
                carRB.isKinematic = false;                
            }
        }
        else
        {
            //sets driving type to car
            carSteer();
            car.steeringWheel.parent = null;

            //enables tank mode
            if (Input.GetKeyDown(KeyCode.F))
            {
                basic.vehicalType = false;
                tankRB.isKinematic = false;
                carRB.isKinematic = true;                
            }
        }

        //allows player to dash
        dashes();
    }

    //tank controls
    void tankSteer()
    {
        //grants the player control of the tanks direction
        transform.Rotate(0, Input.GetAxis("Horizontal") * tank.turnSpeed * Time.deltaTime, 0);
        Vector3 tankDrive = transform.forward * Input.GetAxis("Vertical") * tank.speed;
        tank.movDir = new Vector3(tankDrive.x, -9.81f, tankDrive.z);
        
        //allows the player to junp
        if (Input.GetKeyDown(basic.keyBindings.GetComponent<key>().jump))
        {
            tankRB.AddForce(transform.up * dash.dash * 2, ForceMode.Impulse);
        }

        //moves the tank in tank mode
        tankRB.AddForce(tank.movDir);
        tank.jump = tankRB.velocity.y;
    }

    //car controls
    void carSteer()
    {
        //how the car moves in car mode
        car.moveInput = Input.GetAxisRaw("Vertical");
        car.turnInput = Input.GetAxisRaw("Horizontal");
        car.moveInput *= car.moveInput > 0 ? car.speed : car.reverse;
        transform.position = car.steeringWheel.position;
        carRB.AddForce(transform.forward * car.moveInput, ForceMode.Acceleration);
        float newRotation = car.turnInput * car.turn * Time.deltaTime;
        transform.Rotate(0, newRotation, 0, Space.World);
        car.steeringWheel.GetChild(0).rotation = transform.rotation;

        //allows the car mode to jump
        if (Input.GetKeyDown(basic.keyBindings.GetComponent<key>().jump))
        {
            carRB.AddForce(transform.up * dash.dash, ForceMode.Impulse);
        }
    }

    //dashes
    void dashes()
    {
        //checks if you can dash and which direction
        if (dash.dashAmount >= 1)
        {
            if (Input.GetKeyDown(basic.keyBindings.GetComponent<key>().dash))
            {
                dash.dashAmount -= 1;
                dash.dashTimer = dash.dashDelay;

                if (Input.GetKey(basic.keyBindings.GetComponent<key>().backwards))
                {
                    tankRB.AddForce(-transform.forward * dash.dash * 10, ForceMode.Impulse);
                    carRB.AddForce(-transform.forward * dash.dash * 5, ForceMode.Impulse);
                } 
                else if (Input.GetKey(basic.keyBindings.GetComponent<key>().rightwards))
                {
                    tankRB.AddForce(transform.right * dash.dash * 10, ForceMode.Impulse);
                    carRB.AddForce(transform.right * dash.dash * 5, ForceMode.Impulse);
                }
                else if (Input.GetKey(basic.keyBindings.GetComponent<key>().leftwards))
                {
                    tankRB.AddForce(-transform.right * dash.dash * 10, ForceMode.Impulse);
                    carRB.AddForce(-transform.right * dash.dash * 5, ForceMode.Impulse);
                }
                else
                {
                    tankRB.AddForce(transform.forward * dash.dash * 10, ForceMode.Impulse);
                    carRB.AddForce(transform.forward * dash.dash * 5, ForceMode.Impulse);
                }
            }
        }

        //restores dash after a set amount of time
        if (dash.dashAmount < dash.dashTotal)
        {
            dash.dashTimer -= 1 * Time.deltaTime;

            if (dash.dashTimer <= 0)
            {
                dash.dashAmount += 1;
                dash.dashTimer = dash.dashDelay;
            }            
        }
    }
}
