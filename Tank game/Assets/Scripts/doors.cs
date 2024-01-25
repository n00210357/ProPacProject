using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doors : MonoBehaviour
{
    public int doorType;
    public bool open;
    public Transform level;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (doorType == 0)
        {
            gate();
        }
        else if (doorType == 1)
        {
            arena();
        }
    }

    void gate()
    {
        if (open == true)
        {
            Destroy(gameObject);
        }
    }

    void arena()
    {
        foreach (GameObject ene in level.GetComponent<levelCon>().spawnedEnemies)
        {
            if (ene == null)
            {
                doorType = 0;
            }
            else
            {
                doorType = 1;
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        open = true;
    }

    private void OnTriggerExit(Collider other)
    {
        open = false;
    }
}
