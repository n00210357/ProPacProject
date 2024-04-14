using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class turUI : MonoBehaviour
{
    public int key;
    public string[] line;
    public Transform cam;

    private TextMeshPro tmPro;

    // Start is called before the first frame update
    void Start()
    {
        tmPro = GetComponent<TextMeshPro>();
        StartCoroutine(LateStart(0.1f));
    }

    //writes guid
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (key != -1)
        {
            line[1] = saveData.keybindings.keys[key].ToString();
        }

        tmPro.text = line[0] + line[1] + line[2];
    }

    //looks at camera
    void Update()
    {
        if (saveData.pause == false)
        {
            transform.LookAt(cam);
            transform.RotateAround(transform.position, transform.up, 180f);
        }
    }
}
