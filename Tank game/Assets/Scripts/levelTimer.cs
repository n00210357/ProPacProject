using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelTimer : MonoBehaviour
{
    public Text text;
    private GameObject level;

    // Update is called once per frame
    void Update()
    {
        if (saveData.pause == false)
        {
            //detects level
            if (level == null)
            {
                level = GameObject.FindGameObjectWithTag("Map");
                text.text = " ";
            }

            //starts time if it exists
            if (level != null)
            {
                if (level.GetComponent<levelCon>().type == 0)
                {
                    text.text = " ";
                }
                else
                {
                    text.text = level.GetComponent<levelCon>().time.ToString();
                }
            }
        }
    }
}
