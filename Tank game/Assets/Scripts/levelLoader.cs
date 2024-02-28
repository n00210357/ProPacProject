using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelLoader : MonoBehaviour
{    
    public bool starter;
    public bool desMap;
    public Transform tunnelEnter;
    public Transform tunnelExit;
    public GameObject map;

    void Start()
    {         
        map = GameObject.FindGameObjectWithTag("Map");

        if (starter == false)
        {
            tunnelEnter.position = map.GetComponent<levelCon>().levCon.position;   
        } 
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (desMap == false)
        {
            desMap = true;
            Destroy(map);           
        
            int t = Random.Range(0, saveData.levels.Length);
            Transform newMap = Instantiate(saveData.levels[t], tunnelExit.position, tunnelExit.rotation);
            transform.parent.transform.parent = newMap;
        }
    }
}
