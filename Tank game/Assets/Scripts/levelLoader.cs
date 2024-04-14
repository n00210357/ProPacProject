using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelLoader : MonoBehaviour
{
    public int nextType;
    public bool starter;
    public bool desMap;
    public Transform tunnelEnter;
    public Transform tunnelExit;
    public Transform board;
    public GameObject map;
    public Material[] icons;

    private int nexLev;

    //cycles through levels
    void Start()
    {
        nexLev = Random.Range(0, saveData.levels.Length);
        map = GameObject.FindGameObjectWithTag("Map");

        // 0 = arena  1 = sur  2 = race  3 = or  4 = random
        nextType = Random.Range(0, 5);        
        board.GetComponent<Renderer>().material = icons[nextType];

        if (nextType == 4)
        {
            nextType = Random.Range(0, 4);
        }
    }

    //selcet a random level
    void Update()
    {
        if (saveData.pause == false)
        {
            if (nextType == 0 || nextType == 1 || nextType == 3)
            {
                if (saveData.levels[nexLev].GetComponent<levelCon>().type != 3)
                {
                    nexLev = Random.Range(0, saveData.levels.Length);
                }
            }
            else
            {
                if (saveData.levels[nexLev].GetComponent<levelCon>().type != 2)
                {
                    nexLev = Random.Range(0, saveData.levels.Length);
                }
            }
        }
    }
        
    //loads the level
    private void OnTriggerEnter(Collider other)
    {
        if (desMap == false)
        {
            desMap = true;            
            Transform newMap = Instantiate(saveData.levels[nexLev], tunnelExit.position, tunnelExit.rotation);
            transform.parent.transform.parent = newMap;
            Destroy(map);

            if (nextType == 0)
            {
                newMap.GetComponent<levelCon>().type = 0;
            }
            else if (nextType == 1)
            {
                newMap.GetComponent<levelCon>().type = 1;
            }                       
        }
    }
}
