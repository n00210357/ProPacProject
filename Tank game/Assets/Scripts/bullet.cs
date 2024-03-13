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

    private Rigidbody rigid;
    private GameObject exp;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range, tarLayer);
        foreach (Collider tar in hitEnemies)
        {
            if (tar.GetComponent<enemy>())
            {
                tar.GetComponent<enemy>().takeDamage(damage);
            }

            if (tar.GetComponent<player>())
            {
                tar.GetComponent<player>().takeDamage(damage);
            }

            if (tar.GetComponent<bullet>())
            {
                tar.GetComponent<bullet>().takeDamage(damage);
            }
        }

        Collider[] colli = Physics.OverlapSphere(transform.position, range, notBull);
        foreach (Collider tar in colli)
        {
            die();
            break;
        }

        rigid.AddForce(transform.forward * speed, ForceMode.VelocityChange);

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        
        lifeTime -= 1 * Time.deltaTime;
    }

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

    void die()
    {
        exp = Instantiate(explode.gameObject, transform.position, transform.rotation);    
        Destroy(exp, 0.4f);        
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
