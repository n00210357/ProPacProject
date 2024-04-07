using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGuns : MonoBehaviour
{
    public BaseVar basic = new BaseVar();
    public GunVar gun = new GunVar();
    public ProVar pro = new ProVar();

    //holds the gun base variables
    [Serializable]
    public class BaseVar
    {
        public float rota = 0;
        public int detect = 0;
        public int gunType = 0;
        public bool spin;
        public Transform enemy;
        public GameObject player;
        public AudioClip sound;
        public AudioSource source;
    }

    //holds the gun setting variables
    [Serializable]
    public class GunVar
    {
        public float timer;
        public float timing;
        public float range;
        public float speed;
        public int index;
        public Color[] c;
        public Transform gunEnd;
        public GameObject bullet;
        public LineRenderer line;
    }

    //holds the projectile stats variables
    [Serializable]
    public class ProVar
    {
        public float speed;
        public float range;
        public float lifeTime;
        public int health;
        public int damage;
        public int type;
        public bool killable;
        public Vector3 scale;
        public GameObject bullet;
        public LayerMask bullLayer;
        public LayerMask tarLayer;
        public LayerMask notBull;
        public ParticleSystem explode;
    }

    private Vector3 m_lastKnownPosition = Vector3.zero;
    private Quaternion m_lookAtRotation;

    // Start is called before the first frame update
    void Start()
    {
        basic.gunType = basic.enemy.GetComponent<enemy>().basic.gunType;
        basic.player = basic.enemy.GetComponent<enemy>().basic.player;

        gun.line = gun.gunEnd.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        basic.detect = basic.enemy.GetComponent<enemy>().basic.detect;
        targeter();

        if (basic.gunType == 0)
        {
            bulletBased();
        }
    }

    //targets the player
    void targeter()
    {
        //spins the turret when it has can't see the player
        if (basic.detect == 0 && basic.spin == true)
        {
            transform.Rotate(0, (gun.speed / 2) * Time.deltaTime, 0);
        }
        else if (basic.detect == 1 && basic.spin == true)
        {
            transform.Rotate(0, gun.speed * Time.deltaTime, 0);
        }
        else
        {
            //turns the turret to look at the player
            if (m_lastKnownPosition != basic.player.transform.position)
            {
                m_lastKnownPosition = basic.player.transform.position;
                m_lookAtRotation = Quaternion.LookRotation(m_lastKnownPosition - transform.position);
            }

            if (basic.player.transform.rotation != m_lookAtRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_lookAtRotation, gun.speed * Time.deltaTime);
                //transform.rotation = new Quaternion(transform.rotation.x * basic.rota, transform.rotation.y, transform.rotation.z, gun.speed);
            }
        }
    }

    //prepares ther bullet and fires
    void bulletBased()
    {
        //arms the guns
        RaycastHit hit;
        if (Physics.Raycast(gun.gunEnd.position, gun.gunEnd.forward, out hit, gun.range, pro.tarLayer))
        {
            gun.line.enabled = true;
            gun.line.positionCount = 2;
            gun.line.SetPosition(0, gun.gunEnd.position);
            gun.line.SetPosition(1, hit.point);
            gun.line.material.color = Color.Lerp(gun.line.material.color, gun.c[gun.index], gun.timing * Time.deltaTime);

            //shoots the gun
            if (gun.index == 2)
            {
                gun.line.material.color = Color.green;
                shoot();
                gun.index = 0;
            }
            else
            {
                gun.timer = Mathf.Lerp(gun.timer, 1f, gun.timing * Time.deltaTime);
            }
        }
        else
        {
            gun.line.positionCount = 0;
            gun.timer = 0;
            gun.index = 0;
            gun.line.material.color = Color.red;
            gun.line.enabled = false;
        }

        //arms the gun
        if (gun.timer > .9f)
        {
            gun.timer = 0;
            gun.index++;
            gun.index = (gun.index >= gun.c.Length) ? 0 : gun.index;
        }
    }

    //fires a projectile
    void shoot()
    {
        basic.source.clip = basic.sound;
        basic.source.volume = 1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[2];
        basic.source.pitch = 1;
        basic.source.loop = false;
        basic.source.Play();

        GameObject bull = Instantiate(pro.bullet, gun.gunEnd.position, gun.gunEnd.rotation);
        bull.transform.localScale = pro.scale;
        bull.GetComponent<bullet>().speed = pro.speed;
        bull.GetComponent<bullet>().range = pro.range;
        bull.GetComponent<bullet>().lifeTime = pro.lifeTime;
        bull.GetComponent<bullet>().health = pro.health;
        bull.GetComponent<bullet>().damage = pro.damage;
        bull.GetComponent<bullet>().type = pro.type;
        bull.GetComponent<bullet>().killable = pro.killable;
        bull.GetComponent<bullet>().bullLayer = pro.bullLayer;
        bull.GetComponent<bullet>().tarLayer = pro.tarLayer;
        bull.GetComponent<bullet>().notBull = pro.notBull;
        bull.GetComponent<bullet>().explode = pro.explode;
    }
}
