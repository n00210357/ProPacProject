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
        public int health;
        public int maxHealth;
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

        basic.health = basic.maxHealth;
    }

    void Update()
    {            
        //sets drive type to tank
        if (basic.vehicalType == false)
        {
            tankSteer();
            car.steeringWheel.parent = transform;

            //enables car mode
            if (Input.GetKeyDown(saveData.keybindings.keys[6]))
            {
                basic.vehicalType = true;
                tankRB.isKinematic = true;
                carRB.isKinematic = false;        
                car.steeringWheel.localPosition = new Vector3(0, 0, 0);  
                car.steeringWheel.localRotation = new Quaternion(0, 0, 0, 0);  
            }
        }
        else
        {
            //sets driving type to car
            carSteer();
            car.steeringWheel.parent = null;

            //enables tank mode
            if (Input.GetKeyDown(saveData.keybindings.keys[6]))
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
        if (Input.GetKeyDown(saveData.keybindings.keys[4]))
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
    }

    //dashes
    void dashes()
    {
        //checks if you can dash and which direction
        if (dash.dashAmount >= 1)
        {
            //allows the car mode to jump
            if (Input.GetKeyDown(saveData.keybindings.keys[4]))
            {
                carRB.AddForce(transform.up * dash.dash, ForceMode.Impulse);
            }

            if (Input.GetKeyDown(saveData.keybindings.keys[3]))
            {
                dash.dashAmount -= 1;
                dash.dashTimer = dash.dashDelay;

                if (Input.GetKey(KeyCode.S))
                {
                    tankRB.AddForce(-transform.forward * dash.dash * 10, ForceMode.Impulse);
                    carRB.AddForce(-transform.forward * dash.dash * 10, ForceMode.Impulse);
                } 
                else if (Input.GetKey(KeyCode.D))
                {
                    tankRB.AddForce(transform.right * dash.dash * 10, ForceMode.Impulse);
                    carRB.AddForce(transform.right * dash.dash * 10, ForceMode.Impulse);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    tankRB.AddForce(-transform.right * dash.dash * 10, ForceMode.Impulse);
                    carRB.AddForce(-transform.right * dash.dash * 10, ForceMode.Impulse);
                }
                else
                {
                    tankRB.AddForce(transform.forward * dash.dash * 10, ForceMode.Impulse);
                    carRB.AddForce(transform.forward * dash.dash * 10, ForceMode.Impulse);
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
    public void takeDamage(int dam)
    {
        basic.health -= dam;

        if (basic.health <= 0)
        {
            Debug.Log("Dead");
        }
    }
}
