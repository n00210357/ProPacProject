using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public BaseVar basic = new BaseVar();


    //holds the enemies base variables
    [Serializable]
    public class BaseVar
    {
        public int enemyType = 0;
        public bool groEne = false;
        public bool flyEne = false;
        public GameObject level;
        public GameObject player;
    }

    // Start is called before the first frame update
    void Start()
    {
        basic.level = GameObject.FindGameObjectWithTag("Map");
        basic.player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void groundTroop()
    {

    }
}
