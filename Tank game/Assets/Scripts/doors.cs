using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doors : MonoBehaviour
{
    public float speed;
    public int doorType;
    public bool open;
    public bool tunnelExit;
    public Transform openned;
    public Transform closed;
    public GameObject oldTunnel;
    public GameObject level;
    public GameObject player;
    public AudioClip sound;
    public AudioSource source;

    private bool played;

    // Start is called before the first frame update
    void Start()
    {
        played = true;
        if (gameObject.transform.parent.transform.parent.tag != "Tunnel")
        {
            oldTunnel = GameObject.FindGameObjectWithTag("Tunnel");
        }

        level = GameObject.FindGameObjectWithTag("Map");
        player = GameObject.FindGameObjectWithTag("Player");
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (level != null)
        {
            if (level.GetComponent<levelCon>().type == 1 && level.GetComponent<levelCon>().time >= 1 && tunnelExit == false)
            {
                doorType = 2;
            }

            if (level.GetComponent<levelCon>().type == 2)
            {
                doorType = 0;
            }
        }

        if (doorType == 0)
        {
            gate();
        }
        else if (doorType == 1)
        {
            arena();
        }
        else if (doorType == 2)
        {
            survival();
        }
    }

    //open the door
    void gate()
    {
        if (open == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, openned.position, speed);
           
            if (played == false)
            {
                source.clip = sound;
                source.volume = 0.25f * saveData.keybindings.noise[0] * saveData.keybindings.noise[4];
                source.pitch = 3;
                source.loop = false;
                source.PlayOneShot(sound);
                played = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closed.position, speed);

            if (played == false)
            {
                source.clip = sound;
                source.volume = 0.1f * saveData.keybindings.noise[0] * saveData.keybindings.noise[4];
                source.pitch = 3;
                source.loop = false;
                source.PlayOneShot(sound);
                played = true;
            }
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

    //allows the door to open after the timer ends
    void survival()
    {
        if (level.GetComponent<levelCon>().time <= 0)
        {
            doorType = 0;
        }
    }

    //detect when player is at door
    private void OnTriggerEnter(Collider other)
    {
        open = true;
        played = false;
    }

    //detect when player leaves door
    private void OnTriggerExit(Collider other)
    {
        open = false;
        played = false;
    }
}
