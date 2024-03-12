using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class saveData : MonoBehaviour
{
    public static saveData instance;

    public static keybindings keybindings;
    public keybindings keybinds;
    public static upgrades upgrades;
    public upgrades upgrad;
    public static int difficulty;
    public int diff;
    public static Transform[] levels;
    public Transform[] maps;
    public GameObject[] player;

    public Camera camer;

    void Start()
    {
        levels = maps;
        difficulty = diff;

        if (keybindings != null && keybinds == null)
        {
            keybinds = keybindings;
        }

        if (keybindings == null && keybinds != null)
        {
            keybindings = keybinds;
        }

         if (keybinds.keys[0] == KeyCode.None)
        {
            keybinds.keys[0] = KeyCode.Mouse0;
        }

        if (keybinds.keys[1] == KeyCode.None)
        {
            keybinds.keys[1] = KeyCode.Mouse1;
        }

        if (keybinds.keys[2] == KeyCode.None)
        {
            keybinds.keys[2] = KeyCode.Mouse2;
        }

        if (keybinds.keys[3] == KeyCode.None)
        {
            keybinds.keys[3] = KeyCode.LeftShift;
        }

        if (keybinds.keys[4] == KeyCode.None)
        {
            keybinds.keys[4] = KeyCode.Space;
        }

        if (keybinds.keys[5] == KeyCode.None)
        {
            keybinds.keys[5] = KeyCode.LeftControl;
        }

        if (keybinds.keys[6] == KeyCode.None)
        {
            keybinds.keys[6] = KeyCode.Q;
        }

        if (keybinds.keys[7] == KeyCode.None)
        {
            keybinds.keys[7] = KeyCode.R;
        }

        if (keybinds.keys[8] == KeyCode.None)
        {
            keybinds.keys[8] = KeyCode.F;
        }

        if (keybinds.keys[9] == KeyCode.None)
        {
            keybinds.keys[9] = KeyCode.Escape;
        }

        if (keybindings.cam[0] == 0)
        {
            keybindings.cam[0] = 90;
        }

        if (keybindings.cam[1] == 0)
        {
            keybindings.cam[1] = 1;
        }

        if (keybindings.cam[2] == 0)
        {
            keybindings.cam[2] = 10;
        }

        if (keybindings.cam[3] == 0)
        {
            keybindings.cam[3] = 5;
        }

        if (upgrades != null && upgrad == null)
        {
            upgrad = upgrades;
        }

        if (upgrades == null && upgrad != null)
        {
            upgrades = upgrad;
        }
    }

    void Update()
    {
        camer.fieldOfView = keybindings.cam[0];
        RenderSettings.ambientIntensity = keybindings.cam[1];
        difficulty = diff;

        if (Input.GetKeyDown(saveData.keybindings.keys[9]))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                player[0].GetComponent<player>().enabled = false;
                player[0].GetComponent<UIcon>().enabled = false;
                player[1].GetComponent<guns>().enabled = false;
            }
            else
            {
                Time.timeScale = 1;
                player[0].GetComponent<player>().enabled = true;
                player[0].GetComponent<UIcon>().enabled = true;
                player[1].GetComponent<guns>().enabled = true;
            }
        }
    }
}
