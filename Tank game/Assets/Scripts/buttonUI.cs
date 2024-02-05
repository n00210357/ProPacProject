using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonUI : MonoBehaviour
{
    public int buttonID = 0;
    public Text tex;
    public Transform promt;

    private bool scan = false;

    void Start()
    {
        if (buttonID == -1)
        {
            promt.GetComponent<Slider>().minValue = 0;
            promt.GetComponent<Slider>().maxValue = 179;
            promt.GetComponent<Slider>().value = saveData.cam[0];
        }
        else if (buttonID == -2)
        {
            promt.GetComponent<Slider>().minValue = 0;
            promt.GetComponent<Slider>().maxValue = 8;
            promt.GetComponent<Slider>().value = saveData.cam[1];
        }
        else if (buttonID <= -3)
        {
            promt.GetComponent<Slider>().minValue = 0;
            promt.GetComponent<Slider>().maxValue = 1;

            if (buttonID == -3)
            {
                promt.GetComponent<Slider>().value = saveData.noise[0];
            }
            else if (buttonID == -4)
            {
                promt.GetComponent<Slider>().value = saveData.noise[1];
            }
            else if (buttonID == -5)
            {
                promt.GetComponent<Slider>().value = saveData.noise[2];
            }
            else if (buttonID == -6)
            {
                promt.GetComponent<Slider>().value = saveData.noise[3];
            }
            else if (buttonID == -7)
            {
                promt.GetComponent<Slider>().value = saveData.noise[4];
            }
        }
    }

    public void Update()
    {
        if (buttonID >= 0)
        {
            tex.text = saveData.keybindings.keys[buttonID].ToString();

            if (scan == true)
            {
                promt.gameObject.SetActive(true);
            }
        }
    }

    public void keyBinding()
    {
        scan = true;
    }

    void OnGUI()
    {
        if (buttonID == -1)
        {
            saveData.cam[0] = promt.GetComponent<Slider>().value;
            tex.text = saveData.cam[0].ToString();
        }

        if (buttonID == -2)
        {
            saveData.cam[1] = promt.GetComponent<Slider>().value;
            tex.text = saveData.cam[1].ToString();
        }

        if (buttonID <= -3)
        {
            saveData.noise[0] = promt.GetComponent<Slider>().value;
            tex.text = saveData.noise[0].ToString();
        }

        if (buttonID <= -4)
        {
            saveData.noise[1] = promt.GetComponent<Slider>().value;
            tex.text = saveData.noise[1].ToString();
        }

        if (buttonID <= -5)
        {
            saveData.noise[2] = promt.GetComponent<Slider>().value;
            tex.text = saveData.noise[2].ToString();
        }

        if (buttonID <= -6)
        {
            saveData.noise[3] = promt.GetComponent<Slider>().value;
            tex.text = saveData.noise[3].ToString();
        }

        if (buttonID <= -7)
        {
            saveData.noise[4] = promt.GetComponent<Slider>().value;
            tex.text = saveData.noise[4].ToString();
        }

        if (buttonID <= -8)
        {
            saveData.noise[5] = promt.GetComponent<Slider>().value;
            tex.text = saveData.noise[5].ToString();
        }

        if (buttonID <= -9)
        {
            saveData.noise[6] = promt.GetComponent<Slider>().value;
            tex.text = saveData.noise[6].ToString();
        }

        if (scan == true)
        {
            Event e = Event.current;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                saveData.keybindings.keys[buttonID] = KeyCode.LeftShift;
                promt.gameObject.SetActive(false);
                scan = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightShift))
            {
                saveData.keybindings.keys[buttonID] = KeyCode.RightShift;
                promt.gameObject.SetActive(false);
                scan = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                saveData.keybindings.keys[buttonID] = KeyCode.Mouse0;
                promt.gameObject.SetActive(false);
                scan = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                saveData.keybindings.keys[buttonID] = KeyCode.Mouse1;
                promt.gameObject.SetActive(false);
                scan = false;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                saveData.keybindings.keys[buttonID] = KeyCode.Mouse2;
                promt.gameObject.SetActive(false);
                scan = false;
            }
            if ((e.isKey))
            {
                saveData.keybindings.keys[buttonID] = e.keyCode;
                promt.gameObject.SetActive(false);
                scan = false;
            }
        }

    }
}
