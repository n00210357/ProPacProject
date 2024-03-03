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
        public float dashAmount;
        public float dashTotal;
        public float thrust;
    }

    //holds the tank modes variables
    [Serializable]
    public class TankVar
    {
        public float speed;
        public float turnSpeed;
        public float jump;
        public float gravity;
        public float touchDown;
        public Vector3 movDir;
        public LayerMask land;
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
        public Transform tread;
    }

    private Quaternion roty;
    private Rigidbody tankRB;
    private Rigidbody carRB;
    private Rigidbody baseRB;
    private RaycastHit grav;

    void Start()
    {        
        //assigns the rigidbodies
        tankRB = GetComponent<Rigidbody>();
        carRB = car.steeringWheel.GetComponent<Rigidbody>();
        baseRB = car.tread.GetComponent<Rigidbody>();
    }

    void FixedStart()
    {
        upgrades();
        basic.health = basic.maxHealth;
        basic.vehicalType = false;
    }

    void Update()
    {
        upgrades();
        angle();

        //sets drive type to tank
        if (basic.vehicalType == false)
        {
            tankSteer();
            car.steeringWheel.parent = transform;
            car.tread.position = transform.position;
            car.tread.rotation = transform.rotation;

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
            car.tread.position = transform.position;

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

    void angle()
    {
        tankRB.AddTorque(transform.up * 1000f * Time.deltaTime);
    }

    //tank controls
    void tankSteer()
    {
        gravize();

        //grants the player control of the tanks direction
        transform.Rotate(0, Input.GetAxis("Horizontal") * tank.turnSpeed * Time.deltaTime, 0);
        Vector3 tankDrive = transform.forward * Input.GetAxis("Vertical") * tank.speed * Time.deltaTime;
        tank.movDir = new Vector3(tankDrive.x, tank.gravity, tankDrive.z);

        //moves the tank in tank mode
        tankRB.AddForce(tank.movDir);
        tank.jump = tankRB.velocity.y * Time.deltaTime;
    }

    void gravize()
    {
        if (Physics.Raycast(transform.position, -transform.up, out grav, tank.touchDown, tank.land))
        {
            tank.gravity = 0;
        }
        else
        {
            tank.gravity = -9.81f;
        }
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
                tankRB.AddForce(transform.up * dash.dash * 200, ForceMode.Impulse);
                carRB.AddForce(transform.up * dash.dash, ForceMode.Impulse);
                dash.dashAmount -= dash.thrust;
                dash.dashTimer = dash.dashDelay;
            }
            
            if (basic.vehicalType == true &&Input.GetKey(saveData.keybindings.keys[3]))
            {
                dash.dashAmount -= 3 * Time.deltaTime;

                if (Input.GetKey(KeyCode.S))
                {
                    carRB.AddForce(-transform.forward * dash.dash * 3, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    carRB.AddForce(transform.right * dash.dash * 3, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    carRB.AddForce(-transform.right * dash.dash * 3, ForceMode.Acceleration);
                }
                else
                {
                    carRB.AddForce(transform.forward * dash.dash * 3, ForceMode.Acceleration);
                }
            }
            else if (basic.vehicalType == false && Input.GetKeyDown(saveData.keybindings.keys[3]) && dash.dashAmount >= 0)
            {
                dash.dashAmount -= dash.thrust;
                dash.dashTimer = dash.dashDelay;

                if (Input.GetKey(KeyCode.S))
                {
                    tankRB.AddForce(-transform.forward * dash.dash * 1000, ForceMode.Impulse);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    tankRB.AddForce(transform.right * dash.dash * 1000, ForceMode.Impulse);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    tankRB.AddForce(-transform.right * dash.dash * 1000, ForceMode.Impulse);
                }
                else
                {
                    tankRB.AddForce(transform.forward * dash.dash * 1000, ForceMode.Impulse);
                }
            }                             
        }

        //restores dash after a set amount of time
        if (dash.dashAmount < 0)
        {
            dash.dashAmount = 0; ;
        }

        if (dash.dashAmount < dash.dashTotal)
        {
            dash.dashTimer -= 1 * Time.deltaTime;
        }

        if (dash.dashTimer <= 0)
        {
            dash.dashAmount += 1 * Time.deltaTime;
        }

        if (dash.dashAmount >= dash.dashTotal)
        {
            dash.dashAmount = dash.dashTotal;
            dash.dashTimer = dash.dashDelay;
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

    void upgrades()
    {
        if (saveData.upgrades.health[saveData.upgrades.health.Length - 1] == false)
        {
            if (saveData.upgrades.health[basic.maxHealth + 1] == true)
            {
                basic.maxHealth += 1;
            }
        }

        if (saveData.upgrades.health[basic.maxHealth - 1] == false)
        {
            basic.maxHealth -= 1;
        }

        saveData.upgrades.health[0] = true;
    }
}
