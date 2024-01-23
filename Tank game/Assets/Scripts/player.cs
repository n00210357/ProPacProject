using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public BaseVar basic = new BaseVar();
    public TankVar tank = new TankVar();
    public CarVar car = new CarVar();

    //holds the players base variables
    [Serializable]
    public class BaseVar
    {
        public bool vehicalType = false;
    }

    //holds the tank modes variables
    [Serializable]
    public class TankVar
    {
        public float speed;
        public float turnSpeed;
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
        tankRB = GetComponent<Rigidbody>();
        carRB = car.steeringWheel.GetComponent<Rigidbody>();
    }

    void Update()
    {            
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
    }

    //tank controls
    void tankSteer()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * tank.turnSpeed * Time.deltaTime, 0);
        Vector3 movDir = transform.forward * Input.GetAxis("Vertical") * tank.speed;
        tankRB.velocity = movDir * Time.deltaTime;
    }

    //car controls
    void carSteer()
    {
        car.moveInput = Input.GetAxisRaw("Vertical");
        car.turnInput = Input.GetAxisRaw("Horizontal");
        car.moveInput *= car.moveInput > 0 ? car.speed : car.reverse;
        transform.position = car.steeringWheel.position;
        carRB.AddForce(transform.forward * car.moveInput, ForceMode.Acceleration);
        float newRotation = car.turnInput * car.turn * Time.deltaTime;
        transform.Rotate(0, newRotation, 0, Space.World);
    }
}
