using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    public float speed;
    public int move = 0;
    public bool inGame;
    public Transform page;
    public Transform[] postions;

    private bool pause = false;

    void Update()
    {
        page.position = Vector3.MoveTowards(page.position, postions[move].position, speed * Time.unscaledDeltaTime);

        if (inGame == true)
        {
            if (Input.GetKeyDown(saveData.keybindings.keys[9]) && Time.timeScale == 1)
            {
                move = 0;
                pause = false;
            }
            else if (Input.GetKeyDown(saveData.keybindings.keys[9]) && Time.timeScale == 0)
            {
                move = 1;
            }
        }
    }

    public void moveUI(int moving)
    {
        move = moving;

        if (pause == true)
        {
            Time.timeScale = 1;    
        }
    }

    public void unPause()
    {
        pause = true;
    }

    public void quitGame()
    {
        Application.Quit();
    }
}

