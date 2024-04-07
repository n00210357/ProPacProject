using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class menu : MonoBehaviour
{
    public float speed;
    public int move = 0;
    public bool inGame;
    public Transform pla;
    public Transform page;
    public Transform[] postions;
    public AudioClip sound;
    public AudioSource source;

    private bool pause = false;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    //turns page
    void Update()
    {
        page.position = Vector3.MoveTowards(page.position, postions[move].position, speed * Time.unscaledDeltaTime);

        if (inGame == true)
        {
            if (Input.GetKeyDown(saveData.keybindings.keys[9]) && Time.timeScale == 1)
            {
                move = 1;
                pause = false;
            }
            else if (Input.GetKeyDown(saveData.keybindings.keys[9]) && Time.timeScale == 0)
            {
                move = 0;
            }
        }

        if (pla != null)
        {
            if (pla.GetComponent<player>().basic.dead == true)
            {
                pause = false;
                move = 7;
            }
        }

        cursorCon();
    }

    //moves level
    public void moveUI(int moving)
    {
        move = moving;
        source.clip = sound;
        source.volume = saveData.keybindings.noise[0] * saveData.keybindings.noise[4];
        source.pitch = 1;
        source.loop = false;
        source.Play();

        if (pause == true)
        {
            Time.timeScale = 1;    
        }
    }

    //pauses
    public void unPause()
    {
        pause = true;
    }

    void cursorCon()
    {
        // Hides the cursor
        if (saveData.UI == false && pause == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    //quits the game
    public void quitGame()
    {
        Application.Quit();
    }
}

