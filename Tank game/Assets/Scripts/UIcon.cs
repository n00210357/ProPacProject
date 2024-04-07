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
    public ModeVar mode = new ModeVar();

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

    //cann UI
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

    //sec UI
    [Serializable]
    public class SecCanVar
    {
        public float maxAmmo;
        public int ammo;
        public Image ammoBar;
    }

    //dash UI
    [Serializable]
    public class DashVar
    {
        public float maxFuel;
        public float fuel;
        public Image fuelBar;
        public Text liter;
    }

    //mode UI
    [Serializable]
    public class ModeVar
    {
        public bool type;
        public Image car;
        public Image tank;
    }

    // Start is called before the first frame update
    void Start()
    {       
        hea.hearts[0] = hea.firstHeart;

        main.delay = basic.guns.GetComponent<guns>().main.delay;
        main.ammoBar = main.ammoBar.GetComponent<Image>();
        main.maxAmmo = basic.guns.GetComponent<guns>().main.maxAmmo;

        sec.ammoBar = sec.ammoBar.GetComponent<Image>();

        dash.fuelBar = dash.fuelBar.GetComponent<Image>();
        mode.car.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthTrack();
        primaryCannon();
        secondaryGun();
        dashTrack();
        modeTrack();
        
    }

    //tracks health
    void healthTrack()
    {
        //adds or removes max health
        if (hea.maxHealth != GetComponent<player>().basic.maxHealth)
        {
            hea.maxHealth = 1;

            foreach(Transform hear in hea.hearts)
            {
                if (hear != hea.firstHeart)
                {
                    hear.gameObject.SetActive(false);
                    Destroy(hear.gameObject);
                }
            } 

            if (hea.maxHealth <= GetComponent<player>().basic.maxHealth)
            {
                hea.hearts = new Transform[GetComponent<player>().basic.maxHealth];
                hea.hearts[0] = hea.firstHeart;

                for (int i = 1; i < GetComponent<player>().basic.maxHealth; i++)
                {
                    hea.hearts[i] = Instantiate(hea.firstHeart);
                    hea.hearts[i].transform.SetParent(hea.heartHolder);
                    hea.hearts[i].position = new Vector3(hea.hearts[0].position.x + (hea.dist * i), hea.hearts[0].position.y, hea.hearts[0].position.z);
                }
            }

            hea.maxHealth = GetComponent<player>().basic.maxHealth;
        }

        //colours health
        Color tempColor;
        foreach (Transform he in hea.hearts)
        {
            tempColor = he.GetComponent<Image>().color;
            tempColor.a = 0.15f;
            he.GetComponent<Image>().color = tempColor;
        }

        for (int i = 0; i < transform.GetComponent<player>().basic.health; i++)
        {
            tempColor = hea.hearts[i].GetComponent<Image>().color;
            tempColor.a = 1f;
            hea.hearts[i].GetComponent<Image>().color = tempColor;
        }
    }

    //tracks cannon
    void primaryCannon()
    {        
        //reload icons
        main.reload = basic.guns.GetComponent<guns>().main.reload;
        main.shells[0] = main.firstShell;

        if (main.reload != 0)
        {
            main.ammoBar.fillAmount = main.reload / main.delay;
        }
        else
        {
            main.ammoBar.fillAmount = 1;
        }

        //adds or removes ammo icon
        if (main.maxAmmo != basic.guns.GetComponent<guns>().main.maxAmmo)
        {          
            foreach (Transform she in main.shells)
            {
                if (she != main.firstShell && she != null)
                {
                    Destroy(she.gameObject);
                }
            }     
            
            if (main.maxAmmo != basic.guns.GetComponent<guns>().main.maxAmmo)
            {
                main.shells = new Transform[basic.guns.GetComponent<guns>().main.maxAmmo];
                main.shells[0] = main.firstShell;
                
                for (int i = 1; i < basic.guns.GetComponent<guns>().main.maxAmmo; i++)
                {
                    main.shells[i] = Instantiate(main.firstShell);
                    main.shells[i].transform.SetParent(main.shellHolder);
                    main.shells[i].position = new Vector3(main.shells[0].position.x + (main.dist * i), main.shells[0].position.y, main.shells[0].position.z);
                }
            }

            main.maxAmmo = basic.guns.GetComponent<guns>().main.maxAmmo;
        }
           
        //colours ammo icon
        Color tempColor;
        foreach (Transform shell in main.shells)
        {
            if (shell != null)
            {
                tempColor = shell.GetComponent<Image>().color;
                tempColor.a = 0.15f;
                shell.GetComponent<Image>().color = tempColor;
            }
        }

        for (int i = 0; i < basic.guns.GetComponent<guns>().main.ammo; i++)
        {
            if (main.shells[i] != null)
            {
                tempColor = main.shells[i].GetComponent<Image>().color;
                tempColor.a = 1f;
                main.shells[i].GetComponent<Image>().color = tempColor;
            }
        } 
    }

    //sec UI
    void secondaryGun()
    {
        //fills ammo bar
        sec.maxAmmo = basic.guns.GetComponent<guns>().sec.maxAmmo;
        sec.ammo = basic.guns.GetComponent<guns>().sec.ammo;
        sec.ammoBar.fillAmount = sec.ammo / sec.maxAmmo;

        Color tempColor = sec.ammoBar.GetComponent<Image>().color;

        //ammo colour
        if (sec.maxAmmo >= 600 && sec.maxAmmo <= 1400)
        {
            tempColor.g = 0.75f;
            tempColor.b = 0.75f;            
        }
        else if (sec.maxAmmo >= 1401 && sec.maxAmmo <= 1900)
        {
            tempColor.g = 0.5f;
            tempColor.b = 0.5f;
        }
        else if (sec.maxAmmo >= 1901)
        {
            tempColor.g = 0.25f;
            tempColor.b = 0.25f;
        }
        else
        {
            tempColor.g = 1f;
            tempColor.b = 1f;
        }

        sec.ammoBar.GetComponent<Image>().color = tempColor;
    }

    //dash UI
    void dashTrack()
    {
        //fills dash bar
        dash.maxFuel = GetComponent<player>().dash.dashTotal;
        dash.fuel = GetComponent<player>().dash.dashAmount;
        dash.fuelBar.fillAmount = dash.fuel / dash.maxFuel;
        dash.liter.text = (dash.maxFuel / 10).ToString();
    }

    //mode UI
    void modeTrack()
    {
        //swaps mode icon
        mode.type = GetComponent<player>().basic.vehicalType;

        if (mode.type == true)
        {
            mode.tank.enabled = false;
            mode.car.enabled = true;
        }
        else
        {
            mode.tank.enabled = true;
            mode.car.enabled = false;
        }
    }
}
