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
        public bool dead;
        public bool moving;
        public bool vehicalType = false;
        public GameObject lights;
        public GameObject keyBindings;
        public AudioClip[] sounds;
        public AudioSource source;
    }

    //hold the dash variables
    [Serializable]
    public class DashVar
    {
        public float dash;
        public float dashTimer;
        public float dashDelay;
        public float dashWait;
        public float dashAmount;
        public float thrust;
        public int dashTotal;
        public Transform[] nitroHolders;
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
        public Transform treads;
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
        public Transform[] hudCaps;
    }

    private float xRot;
    private bool played;
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
        basic.source = GetComponent<AudioSource>();
        basic.vehicalType = false;
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (saveData.keybindings.health != 0 && saveData.upgrades.health[1] == true)
        {
            basic.health = saveData.keybindings.health;
        }
    }

    void Update()
    {
        if (saveData.pause == false)
        {
            if (basic.dead == false)
            {
                angle();

                //activates lights
                if (Input.GetKeyDown(saveData.keybindings.keys[8]) && basic.lights.activeInHierarchy == false)
                {
                    basic.lights.SetActive(true);
                }
                else if (Input.GetKeyDown(saveData.keybindings.keys[8]))
                {
                    basic.lights.SetActive(false);
                }

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
                        basic.source.Stop();
                        basic.vehicalType = true;
                        tankRB.isKinematic = true;
                        carRB.isKinematic = false;
                        car.steeringWheel.localPosition = new Vector3(0, 0, 0);
                        car.steeringWheel.localRotation = new Quaternion(0, 0, 0, 0);
                        tank.treads.localPosition = new Vector3(tank.treads.localPosition.x, 0.09f, tank.treads.localPosition.z);
                        foreach (Transform hud in car.hudCaps)
                        {
                            hud.localPosition = new Vector3(hud.localPosition.x, -0.2f, hud.localPosition.z);
                        }
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
                        basic.source.Stop();
                        basic.vehicalType = false;
                        tankRB.isKinematic = false;
                        carRB.isKinematic = true;
                        tank.treads.localPosition = new Vector3(tank.treads.localPosition.x, 0, tank.treads.localPosition.z);
                        foreach (Transform hud in car.hudCaps)
                        {
                            hud.localPosition = new Vector3(hud.localPosition.x, 0, hud.localPosition.z);
                        }
                    }
                }

                //allows player to dash
                dashes();
            }
        }
    }  

    void FixedUpdate()
    {
        upgrades();
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

        if (tank.movDir.x != 0 || tank.movDir.z != 0)
        {
            if (tank.treads.localPosition.z >= 0.15f)
            {
                basic.moving = true;
            }                
                
            if (tank.treads.localPosition.z <= -0.15f)
            {
                basic.moving = false;
            }  

            //animates treads
            if (basic.moving == true)
            {
                tank.treads.localPosition = new Vector3(tank.treads.localPosition.x, tank.treads.localPosition.y, tank.treads.localPosition.z - 2 * Time.deltaTime);
            }
            else
            {
                tank.treads.localPosition = new Vector3(tank.treads.localPosition.x, tank.treads.localPosition.y, tank.treads.localPosition.z + 2 * Time.deltaTime);
            }

            //playes sound
            if (played == false)
            {
                basic.source.clip = basic.sounds[0];
                basic.source.volume = 0.25f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1];
                basic.source.pitch = 1;
                basic.source.loop = true;
                basic.source.Play();
                played = true;
            }
        }
        else if (Input.GetAxis("Horizontal") != 0)
        {
            if (played == false)
            {
                basic.source.clip = basic.sounds[0];
                basic.source.volume = 0.25f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1];
                basic.source.pitch = 1;
                basic.source.loop = true;
                basic.source.Play();
                played = true;
            }
        }
        else if (tank.movDir.x == 0 && tank.movDir.z == 0 && Input.GetAxis("Horizontal") == 0 && played == true)
        {
            basic.source.Stop();
            played = false;
        }

        //moves the tank in tank mode
        tankRB.AddForce(tank.movDir);
        tank.jump = tankRB.velocity.y * Time.deltaTime;
    }
    //activates gravity
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

        //plays car sound
        if (car.moveInput != 0 || car.turnInput != 0)
        {
            if (played == false)
            {
                basic.source.clip = basic.sounds[1];
                basic.source.volume = 0.4f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1];
                basic.source.pitch = 1;
                basic.source.loop = true;
                basic.source.Play();
                played = true;
            }
        }
        else if (played == true)
        {
            played = false;
            basic.source.Stop();
        }

        if (Input.GetKey(saveData.keybindings.keys[5]))
        {
            car.turn = 180f;
            car.speed = 25;
        }
        else
        {
            car.turn = 90f;
            car.speed = 50;
        }

        //animates wheels
        if (car.turnInput < 0)
        {
            car.hudCaps[0].GetChild(1).localRotation = Quaternion.Euler(xRot += 5f, -45, car.hudCaps[0].GetChild(1).localRotation.z);
            car.hudCaps[1].GetChild(1).localRotation = Quaternion.Euler(xRot += 5f, -45, car.hudCaps[1].GetChild(1).localRotation.z);
        }
        else if (car.turnInput > 0)
        {
            car.hudCaps[0].GetChild(1).localRotation = Quaternion.Euler(xRot += 5f, 45, car.hudCaps[0].GetChild(1).localRotation.z);
            car.hudCaps[1].GetChild(1).localRotation = Quaternion.Euler(xRot += 5f, 45, car.hudCaps[1].GetChild(1).localRotation.z);
        }
        else
        {
            if (car.moveInput != 0)
            {
                car.hudCaps[0].GetChild(1).localRotation = Quaternion.Euler(xRot += 5f, 0, car.hudCaps[0].GetChild(1).localRotation.z);
                car.hudCaps[1].GetChild(1).localRotation = Quaternion.Euler(xRot += 5f, 0, car.hudCaps[1].GetChild(1).localRotation.z);
            }
            else
            {
                car.hudCaps[0].GetChild(1).localRotation = Quaternion.Euler(xRot, 0, car.hudCaps[0].GetChild(1).localRotation.z);
                car.hudCaps[1].GetChild(1).localRotation = Quaternion.Euler(xRot, 0, car.hudCaps[1].GetChild(1).localRotation.z);
            }
        }

        if (car.moveInput != 0)
        {
            car.hudCaps[2].GetChild(1).Rotate(-5f, car.hudCaps[2].GetChild(1).localRotation.y, car.hudCaps[2].GetChild(1).localRotation.z, Space.Self);
            car.hudCaps[3].GetChild(1).Rotate(-5f, car.hudCaps[3].GetChild(1).localRotation.y, car.hudCaps[3].GetChild(1).localRotation.z, Space.Self);
        }
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
                carRB.AddForce(transform.up * dash.dash * 2, ForceMode.Impulse);
                dash.dashAmount -= dash.thrust;
                dash.dashTimer = dash.dashDelay;

                basic.source.Stop();
                played = false;
                basic.source.clip = basic.sounds[2];
                basic.source.volume = 1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1];
                basic.source.pitch = 1f;
                basic.source.loop = false;
                basic.source.PlayOneShot(basic.sounds[2], 1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1]);
            }

            //cars nitro
            if (basic.vehicalType == true &&Input.GetKey(saveData.keybindings.keys[3]))
            {
                dash.dashAmount -= 3 * Time.deltaTime;
                if (Input.GetKey(KeyCode.S))
                {
                    for (var i = 0; i < 3; i++)
                    {
                        dash.nitroHolders[4].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                        dash.nitroHolders[5].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    carRB.AddForce(-transform.forward * dash.dash * 3, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    for (var i = 0; i < 3; i++)
                    {
                        dash.nitroHolders[1].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    carRB.AddForce(transform.right * dash.dash * 3, ForceMode.Acceleration);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    for (var i = 0; i < 3; i++)
                    {
                        dash.nitroHolders[0].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    carRB.AddForce(-transform.right * dash.dash * 3, ForceMode.Acceleration);
                }
                else
                {
                    for (var i = 0; i < 3; i++)
                    {
                        dash.nitroHolders[2].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                        dash.nitroHolders[3].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    carRB.AddForce(transform.forward * dash.dash * 3, ForceMode.Acceleration);
                }
            } // tank dash
            else if (basic.vehicalType == false && Input.GetKeyDown(saveData.keybindings.keys[3]) && dash.dashAmount >= 0)
            {
                dash.dashAmount -= dash.thrust;
                dash.dashTimer = dash.dashDelay;

                basic.source.Stop();
                played = false;
                basic.source.clip = basic.sounds[2];
                basic.source.volume = 1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1];
                basic.source.pitch = 3f;
                basic.source.loop = false;
                basic.source.PlayOneShot(basic.sounds[2], 1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1]);

                if (Input.GetKey(KeyCode.S))
                {
                    for (var i = 0; i < 3; i++)
                    {
                        dash.nitroHolders[4].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                        dash.nitroHolders[5].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    tankRB.AddForce(-transform.forward * dash.dash * 1000, ForceMode.Impulse);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    for (var i = 0; i < 3; i++)
                    {
                        dash.nitroHolders[1].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    tankRB.AddForce(transform.right * dash.dash * 1000, ForceMode.Impulse);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    for (var i = 0; i < 3; i++)
                    {
                        dash.nitroHolders[0].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    tankRB.AddForce(-transform.right * dash.dash * 1000, ForceMode.Impulse);
                }
                else 
                {
                    for (var i = 0; i < 3; i++)
                    {
                        dash.nitroHolders[2].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                        dash.nitroHolders[3].GetChild(i).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    tankRB.AddForce(transform.forward * dash.dash * 1000, ForceMode.Impulse);
                }
            }
            
            //plays dash sound
            if (basic.vehicalType == true && Input.GetKeyDown(saveData.keybindings.keys[3]))
            {
                basic.source.Stop();
                played = false;
                basic.source.clip = basic.sounds[2];
                basic.source.volume = 1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1];
                basic.source.pitch = 1f;
                basic.source.loop = false;
                basic.source.PlayOneShot(basic.sounds[2], 1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[1]);
            }
            //actives particles
            foreach(Transform da in dash.nitroHolders)
            {
                for (var i = 0; i < 3; i++)
                {
                    var em = da.GetChild(i).gameObject.GetComponent<ParticleSystem>().main;
                    em.loop = Input.GetKey(saveData.keybindings.keys[3]);
                }
            }

            if ((Input.GetKey(KeyCode.S) == false || dash.dashAmount <= 0) && basic.vehicalType == true)
            {
                for (var i = 0; i < 3; i++)
                {
                    dash.nitroHolders[4].GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
                    dash.nitroHolders[5].GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
            
            if ((Input.GetKey(KeyCode.D) == false || dash.dashAmount <= 0) && basic.vehicalType == true)
            {
                for (var i = 0; i < 3; i++)
                {
                    dash.nitroHolders[1].GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
            
            if ((Input.GetKey(KeyCode.A) == false || dash.dashAmount <= 0) && basic.vehicalType == true)
            {
                for (var i = 0; i < 3; i++)
                {
                    dash.nitroHolders[0].GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
                }
            }
            
            if ((Input.GetKey(KeyCode.W) == false || dash.dashAmount <= 0) && basic.vehicalType == true)
            {
                for (var i = 0; i < 3; i++)
                {
                    dash.nitroHolders[2].GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
                    dash.nitroHolders[3].GetChild(i).gameObject.GetComponent<ParticleSystem>().Stop();
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

    //takes damage
    public void takeDamage(int dam)
    {
        basic.health -= dam;

        if (basic.health <= 0)
        {
            basic.dead = true;
        }
    }

    void upgrades()
    {
        //Dash upgrades
        if (saveData.upgrades.dashRechargeSpeed == true)
        {
            dash.dashDelay = dash.dashWait / 10;
        }
        else
        {
            dash.dashDelay = dash.dashWait;
        }

        saveData.upgrades.dashAmount[0] = true;

        if ((dash.dashTotal / 10) < saveData.upgrades.dashAmount.Length)
        {
            if (saveData.upgrades.dashAmount[(dash.dashTotal / 10) - 1] == true)
            {   
                dash.dashTotal += 10;
            }
        }

        if (saveData.upgrades.dashAmount[(dash.dashTotal / 10) - 1] == false)
        {
            dash.dashTotal -= 10;            
        }

        if (dash.dashAmount > dash.dashTotal)
        {
            dash.dashAmount -= 10;
        }

        //health upgrades
        saveData.upgrades.health[0] = true;

        if (basic.maxHealth < saveData.upgrades.health.Length)
        {
            if (saveData.upgrades.health[basic.maxHealth - 1] == true)
            {
                basic.maxHealth += 1;
            }
        }

        if (saveData.upgrades.health[basic.maxHealth - 1] == false)
        {
            basic.maxHealth -= 1;
        }

        if (basic.health > basic.maxHealth)
        {
            basic.health -= 1;
        }
    }
}
