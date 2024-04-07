using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTran : MonoBehaviour
{
    public string scene;

    //moves to selected scene
    public void Menu()
    {
        SceneManager.LoadScene(scene);
    }

    //resets upgrades
    public void reset()
    {
        saveData.difficulty = 10;
        
        for(int i = 1; i <=  saveData.upgrades.cannShots.Length - 1; i++)
        {
            saveData.upgrades.cannShots[i] = false;
        }

        saveData.upgrades.canReloadSpeed = false;
        saveData.upgrades.shotRadius = false;

        for (int i = 1; i <= saveData.upgrades.machSpeed.Length - 1; i++)
        {
            saveData.upgrades.machSpeed[i] = false;
        }

        for (int i = 1; i <= saveData.upgrades.machAmmo.Length - 1; i++)
        {
            saveData.upgrades.machAmmo[i] = false;
        }

        saveData.upgrades.machReloadSpeed = false;

        for (int i = 1; i <= saveData.upgrades.dashAmount.Length - 1; i++)
        {
            saveData.upgrades.dashAmount[i] = false;
        }

        saveData.upgrades.dashRechargeSpeed = false;

        for (int i = 1; i <= saveData.upgrades.health.Length - 1; i++)
        {
            saveData.upgrades.health[i] = false;
        }
    }
}
