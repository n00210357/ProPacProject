using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcon : MonoBehaviour
{
    public BaseVar basic = new BaseVar();
    public HealthVar hea = new HealthVar();

    //holds the UI base variables
    [Serializable]
    public class BaseVar
    {
        public Transform guns;
    }

    //holds the UI hearths variables
    [Serializable]
    public class HealthVar
    {
        public float dist;
        public int maxHealth;
        public Transform firstHeart;
        public Transform heartHolder;
        public Transform[] hearts;
    }


    // Start is called before the first frame update
    void Start()
    {
        hea.maxHealth = GetComponent<player>().basic.maxHealth;
        hea.hearts = new Transform[hea.maxHealth];
        hea.hearts[0] = (hea.firstHeart);

        if (hea.maxHealth >= 1)
        {
            for(int i = 1; i < hea.maxHealth; i++)
            {
                hea.hearts[i] = Instantiate(hea.firstHeart);
                hea.hearts[i].transform.SetParent(hea.heartHolder);
                hea.hearts[i].position = new Vector3(hea.hearts[0].position.x + (hea.dist * i), hea.hearts[0].position.y, hea.hearts[0].position.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
