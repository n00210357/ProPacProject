using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doors : MonoBehaviour
{
    public float speed;
    public int doorType;
    public bool open;
    public Transform openned;
    public Transform closed;
    public GameObject oldTunnel;
    public GameObject level;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent.transform.parent.tag != "Tunnel")
        {
            oldTunnel = GameObject.FindGameObjectWithTag("Tunnel");
        }

        level = GameObject.FindGameObjectWithTag("Map");
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

    //open the door
    void gate()
    {
        if (open == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, openned.position, speed);
            Destroy(oldTunnel);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closed.position, speed);
        }
    }

    //requares the player to kill all enemies first
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

    //detect when player is at door
    private void OnTriggerEnter(Collider other)
    {
        open = true;
    }

    //detect when player leaves door
    private void OnTriggerExit(Collider other)
    {
        open = false;
    }
}
