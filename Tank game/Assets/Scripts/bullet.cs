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
        public GameObject target;
        public LayerMask bullLayer;
        public LayerMask tarLayer;
        public ParticleSystem explode;

        private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        if (type == 0)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void takeDamage(int dam)
    {
        if (killable == true)
        {
            health -= dam;

            if (health <= 0)
            {
                Debug.Log("Dead");
            }
        }
    }
}
