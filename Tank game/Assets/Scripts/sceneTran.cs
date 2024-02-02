using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTran : MonoBehaviour
{
    public string scene;

    public void Menu()
    {
        SceneManager.LoadScene(scene);
    }
}
