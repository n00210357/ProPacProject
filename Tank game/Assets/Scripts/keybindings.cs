using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "keybindings", menuName = "keybindings")]
public class keybindings : ScriptableObject
{
    public static keybindings instance;
    // 0 = fire1    1 = fire2   2 = fire3  3 = dash  4 = jump  5 = drift  6 = form
    // 7 = reload   8 = lights  9 = pause
    public KeyCode[] keys = new KeyCode[10];

    //0 = fov 1 = bright 2 = xsen 3 = ysen
    public float[] cam = {90, 1, 10, 5};

     //0 = master 1 = player 2 = enemies 3 = music 4 = enviroment
    public float[] noise = {1, 1, 1, 1, 1};
}

