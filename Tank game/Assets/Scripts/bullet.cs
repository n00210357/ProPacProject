using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;
    public float range;
    public float lifeTime;
    public int health;
    public int damage;
    public int type;
    public bool killable;
    public LayerMask bullLayer;
    public LayerMask tarLayer;
    public LayerMask notBull;
    public ParticleSystem explode;
    public AudioClip sound;
    public AudioSource source;

    private Rigidbody rigid;
    private GameObject exp;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //stops the bullet colliding with itself
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

        //allows the bullet to collide with targets
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range, tarLayer);
        foreach (Collider tar in hitEnemies)
        {
            //checks and damages enemies
            if (tar.GetComponent<enemy>())
            {
                tar.GetComponent<enemy>().takeDamage(damage);
            }

            //checks and damages players
            if (tar.GetComponent<player>())
            {
                tar.GetComponent<player>().takeDamage(damage);
            }

            //allows bullets to destroy other bullets
            if (tar.GetComponent<bullet>())
            {
                tar.GetComponent<bullet>().takeDamage(damage);
            }
        }

        //checks if the bullet collides with anything then destroys it and plays particles and sound
        Collider[] colli = Physics.OverlapSphere(transform.position, range, notBull);
        foreach (Collider tar in colli)
        {
            GameObject blasty = new GameObject();
            blasty.transform.position = transform.position;
            blasty.AddComponent<AudioSource>();
            blasty.GetComponent<AudioSource>().clip = sound;
            blasty.GetComponent<AudioSource>().volume = 1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[2];
            blasty.GetComponent<AudioSource>().pitch = 1f;
            blasty.GetComponent<AudioSource>().loop = false;
            blasty.GetComponent<AudioSource>().Play();
            Destroy(blasty, 0.5f);
            die();
            break;
        }

        //moves the bullet
        rigid.AddForce(transform.forward * speed, ForceMode.VelocityChange);

        //destroys the bullet after a set amount of time
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        
        lifeTime -= 1 * Time.deltaTime;
    }

    //allows the bullet to take damage
    public void takeDamage(int dam)
    {
        if (killable == true)
        {
            health -= dam;

            if (health <= 0)
            {
                die();
            }
        }
    }

    //kills the bullet
    void die()
    {
        exp = Instantiate(explode.gameObject, transform.position, transform.rotation);    
        Destroy(exp, 0.4f);        
        Destroy(gameObject);
    }
    
    //draws gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
