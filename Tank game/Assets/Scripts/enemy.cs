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
        public Transform level;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void groundTroop()
    {

    }
}
