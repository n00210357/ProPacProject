using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonUI : MonoBehaviour
{
    public bool started;
    public int buttonID = 0;
    public Text tex;
    public Transform promt;

    private bool scan = false;

    void Start()
    {
        started = false;

        //runs slightly late allowing other start functions to finsh
        StartCoroutine(LateStart(0.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //set ui starting slider positions and values for both sound and graphics
        if (buttonID == -1)
        {
            promt.GetComponent<Slider>().minValue = 5;
            promt.GetComponent<Slider>().maxValue = 110;
            promt.GetComponent<Slider>().value = saveData.keybindings.cam[0];
        }
        else if (buttonID == -2)
        {
            promt.GetComponent<Slider>().minValue = 0;
            promt.GetComponent<Slider>().maxValue = 8;
            promt.GetComponent<Slider>().value = saveData.keybindings.cam[1];
        }
        else if (buttonID <= -3 && buttonID >= -7)
        {
            promt.GetComponent<Slider>().minValue = 0;
            promt.GetComponent<Slider>().maxValue = 1;

            if (buttonID == -3)
            {
                promt.GetComponent<Slider>().value = saveData.keybindings.noise[0];
            }
            else if (buttonID == -4)
            {
                promt.GetComponent<Slider>().value = saveData.keybindings.noise[1];
            }
            else if (buttonID == -5)
            {
                promt.GetComponent<Slider>().value = saveData.keybindings.noise[2];
            }
            else if (buttonID == -6)
            {
                promt.GetComponent<Slider>().value = saveData.keybindings.noise[3];
            }
            else if (buttonID == -7)
            {
                promt.GetComponent<Slider>().value = saveData.keybindings.noise[4];
            }
        }
        else if (buttonID <= -1)
        {
            promt.GetComponent<Slider>().minValue = 0;
            promt.GetComponent<Slider>().maxValue = 25;

            if (buttonID == -8)
            {
                promt.GetComponent<Slider>().value = saveData.keybindings.cam[2];
            }
            else if (buttonID == -9)
            {
                promt.GetComponent<Slider>().value = saveData.keybindings.cam[3];
            }
        }

        started = true;
    }

    public void Update()
    {
        if (buttonID >= 0)
        {
            //show keybind for the keybind settings
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
        if (started == true)
        {
            //allow for the alteration of sound and graphics
            if (buttonID == -1)
            {
                saveData.keybindings.cam[0] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.cam[0].ToString();
            }

            if (buttonID == -2)
            {
                saveData.keybindings.cam[1] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.cam[1].ToString();
            }

            if (buttonID == -3)
            {
                saveData.keybindings.noise[0] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.noise[0].ToString();
            }

            if (buttonID == -4)
            {
                saveData.keybindings.noise[1] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.noise[1].ToString();
            }

            if (buttonID == -5)
            {
                saveData.keybindings.noise[2] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.noise[2].ToString();
            }

            if (buttonID == -6)
            {
                saveData.keybindings.noise[3] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.noise[3].ToString();
            }

            if (buttonID == -7)
            {
                saveData.keybindings.noise[4] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.noise[4].ToString();
            }

            if (buttonID == -8)
            {
                saveData.keybindings.cam[2] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.cam[2].ToString();
            }

            if (buttonID == -9)
            {
                saveData.keybindings.cam[3] = promt.GetComponent<Slider>().value;
                tex.text = saveData.keybindings.cam[3].ToString();
            }

            //allows for keycode binding
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
}
