using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcon : MonoBehaviour
{
    public BaseVar basic = new BaseVar();
    public HealthVar hea = new HealthVar();
    public MainCanVar main = new MainCanVar();
    public SecCanVar sec = new SecCanVar();
    public DashVar dash = new DashVar();
    public AbilityVar abi = new AbilityVar();

    //holds the UI base variables
    [Serializable]
    public class BaseVar
    {
        public Transform guns;
    }

    //holds the UI hearths variables
    [Serializable]
    public class HealthVar
    {
        public float dist;
        public int maxHealth;
        public Transform firstHeart;
        public Transform heartHolder;
        public Transform[] hearts;
    }

    [Serializable]
    public class MainCanVar
    {
        public float delay;
        public float reload;
        public float dist;
        public int maxAmmo;
        public Image ammoBar;
        public Transform firstShell;
        public Transform shellHolder;
        public Transform[] shells;
    }

    [Serializable]
    public class SecCanVar
    {
        public float maxAmmo;
        public int ammo;
        public Image ammoBar;
    }

    [Serializable]
    public class DashVar
    {

    }

    [Serializable]
    public class AbilityVar
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        hea.maxHealth = GetComponent<player>().basic.maxHealth;
        hea.hearts = new Transform[hea.maxHealth];
        hea.hearts[0] = hea.firstHeart;

        if (hea.maxHealth >= 1)
        {
            for(int i = 1; i < hea.maxHealth; i++)
            {
                hea.hearts[i] = Instantiate(hea.firstHeart);
                hea.hearts[i].transform.SetParent(hea.heartHolder);
                hea.hearts[i].position = new Vector3(hea.hearts[0].position.x + (hea.dist * i), hea.hearts[0].position.y, hea.hearts[0].position.z);
            }
        }

        main.delay = basic.guns.GetComponent<guns>().main.delay;
        main.ammoBar = main.ammoBar.GetComponent<Image>();
        main.maxAmmo = basic.guns.GetComponent<guns>().main.maxAmmo;
        main.shells = new Transform[main.maxAmmo];
        main.shells[0] = main.firstShell;

        if (main.maxAmmo >= 1)
        {
            for (int i = 1; i < main.maxAmmo; i++)
            {
                main.shells[i] = Instantiate(main.firstShell);
                main.shells[i].transform.SetParent(main.shellHolder);
                main.shells[i].position = new Vector3(main.shells[0].position.x + (main.dist * i), main.shells[0].position.y, main.shells[0].position.z);
            }
        }

        sec.ammoBar = sec.ammoBar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthTrack();
        primaryCannon();
        secondaryGun();
    }

    void healthTrack()
    {
        Color tempColor;
        foreach (Transform he in hea.hearts)
        {
            tempColor = he.GetComponent<Image>().color;
            tempColor.a = 0.5f;
            he.GetComponent<Image>().color = tempColor;
        }

        for (int i = 0; i < transform.GetComponent<player>().basic.health; i++)
        {
            tempColor = hea.hearts[i].GetComponent<Image>().color;
            tempColor.a = 1f;
            hea.hearts[i].GetComponent<Image>().color = tempColor;
        }
    }

    void primaryCannon()
    {
        main.reload = basic.guns.GetComponent<guns>().main.reload;

        if (main.reload != 0)
        {
            main.ammoBar.fillAmount = main.reload / main.delay;
        }
        else
        {
            main.ammoBar.fillAmount = 1;
        }

        Color tempColor;
        foreach (Transform shell in main.shells)
        {
            tempColor = shell.GetComponent<Image>().color;
            tempColor.a = 0.5f;
            shell.GetComponent<Image>().color = tempColor;
        }

        for (int i = 0; i < basic.guns.GetComponent<guns>().main.ammo; i++)
        {
            tempColor = main.shells[i].GetComponent<Image>().color;
            tempColor.a = 1f;
            main.shells[i].GetComponent<Image>().color = tempColor;
        }
    }

    void secondaryGun()
    {
        sec.maxAmmo = basic.guns.GetComponent<guns>().sec.maxAmmo;
        sec.ammo = basic.guns.GetComponent<guns>().sec.ammo;
        sec.ammoBar.fillAmount = sec.ammo / sec.maxAmmo;
    }
}
