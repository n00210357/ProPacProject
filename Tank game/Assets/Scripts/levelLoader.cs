using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelLoader : MonoBehaviour
{    
    public bool starter;
    public Transform tunnelEnter;
    public Transform tunnelExit;
    public GameObject map;

    void Start ()
    {
        map = GameObject.FindGameObjectWithTag("Map");

        if (starter == false)
        {
            tunnelEnter.position = map.GetComponent<levelCon>().levCon.position;   
        } 
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (starter == false)
        {
            if (map != null)
            {
                Destroy(map);                
            }

        }
    }
}
