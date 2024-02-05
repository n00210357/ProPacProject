using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class saveData : MonoBehaviour
{
    public static int saveFile = 0;
    public static saveData instance;
    public static keyBindings keybindings;
    public keyBindings keybinds;

    //                        infov  bright
    public static float[] cam = { 60, 4 , };
    //                             ma  pl en mu en
    public static float[] noise = { 1, 1, 1, 1, 1};

    public Camera camer;

    void Start()
    {
        if (keybindings != null && keybinds == null)
        {
            keybinds = keybindings;
        }

        if (keybindings == null && keybinds != null)
        {
            keybindings = keybinds;
        }

        cameraSettings();
    }

    void Update()
    {
        cameraSettings();
    }

    void cameraSettings()
    {
        camer.fieldOfView = cam[0];

        RenderSettings.ambientIntensity = cam[1];

    }
}
