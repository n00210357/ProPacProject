using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour
{
    public float speed;
    public int move = 0;
    public Transform page;
    public Transform[] postions;

    void Update()
    {
        page.position = Vector3.MoveTowards(page.position, postions[move].position, speed);
    }

    public void moveUI(int moving)
    {
        move = moving;
    }

    public void quitGame()
    {
        Application.Quit();
    }
}

