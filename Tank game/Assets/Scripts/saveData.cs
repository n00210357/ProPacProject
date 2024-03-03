using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class saveData : MonoBehaviour
{
    public static saveData instance;

    public static KeyBindings keybindings;
    public KeyBindings keybinds;
    public static upgrades upgrades;
    public upgrades upgrad;

    //                          fov  bri xSe  ySe 
    public static float[] cam = { 60, 1, 10, 5};
    //                             ma  pl en mu en
    public static float[] noise = { 1, 1, 1, 1, 1};

    public static int difficulty;
    public int diff;
    public static Transform[] levels;
    public Transform[] maps;
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

        if (upgrades != null && upgrad == null)
        {
            upgrad = upgrades;
        }

        if (upgrades == null && upgrad != null)
        {
            upgrades = upgrad;
        }

        cameraSettings();
    }

    void Update()
    {
        cameraSettings();
        
        difficulty = diff;
    }

    void cameraSettings()
    {
        camer.fieldOfView = cam[0];
        RenderSettings.ambientIntensity = cam[1];
        keybindings.xSen = cam[2];
        keybindings.ySen = cam[3];
    }
}
