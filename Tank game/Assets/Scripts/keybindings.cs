using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "KeyBindings")]
public class KeyBindings : ScriptableObject
{
    public static KeyBindings instance;
    public KeyCode[] keys;
    public float xSen;
    public float ySen;

    // 0 = fire1    1 = fire2   2 = fire3  3 = dash   4 = jump 5 = drift   6 = form
    // 7 = reload
    void start()
    {
        if (keys[0] == KeyCode.None)
        {
            keys[0] = KeyCode.Mouse0;
        }

        if (keys[1] == KeyCode.None)
        {
            keys[1] = KeyCode.Mouse1;
        }

        if (keys[2] == KeyCode.None)
        {
            keys[2] = KeyCode.Mouse2;
        }

        if (keys[3] == KeyCode.None)
        {
            keys[3] = KeyCode.LeftShift;
        }

        if (keys[4] == KeyCode.None)
        {
            keys[4] = KeyCode.Space;
        }

        if (keys[5] == KeyCode.None)
        {
            keys[5] = KeyCode.LeftControl;
        }

        if (keys[6] == KeyCode.None)
        {
            keys[6] = KeyCode.F;
        }

        if (keys[7] == KeyCode.None)
        {
            keys[7] = KeyCode.R;
        }        
    }
}

