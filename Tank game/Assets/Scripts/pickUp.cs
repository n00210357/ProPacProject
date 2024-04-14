using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pickUp : MonoBehaviour
{
    int type = 0;
    public TextMeshPro text;

    void Start()
    {
        type = Random.Range(0, 10);
    }

    //select upgrades
    void Update()
    {
        if (saveData.pause == false)
        {
            if (type == 0)
            {
                if (saveData.upgrades.cannShots[saveData.upgrades.cannShots.Length - 1] == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "cannon shots + 1";
            }
            else if (type == 1)
            {
                if (saveData.upgrades.canReloadSpeed == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "increase reload speed";
            }
            else if (type == 2)
            {
                if (saveData.upgrades.shotRadius == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "increase blast radius";
            }
            else if (type == 3)
            {
                if (saveData.upgrades.machSpeed[saveData.upgrades.machSpeed.Length - 1] == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "machine gun speed";
            }
            else if (type == 4)
            {
                if (saveData.upgrades.machAmmo[saveData.upgrades.machAmmo.Length - 1] == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "increase machine gun ammo";
            }
            else if (type == 5)
            {
                if (saveData.upgrades.machReloadSpeed == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "machine gun reload";
            }
            else if (type == 6)
            {
                if (saveData.upgrades.dashAmount[saveData.upgrades.dashAmount.Length - 1] == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "extra dash";
            }
            else if (type == 7)
            {
                if (saveData.upgrades.dashRechargeSpeed == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "increase dash recharge";
            }
            else if (type == 8)
            {
                if (saveData.upgrades.health[saveData.upgrades.health.Length - 1] == true)
                {
                    type = Random.Range(0, 10);
                }

                text.text = "extra health";
            }
            else if (type == 9)
            {
                saveData.difficulty += 10;
                Destroy(gameObject);
            }
        }
    }

    //upgrades the player
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {            
            if (type == 0)
            {
                for (int i = 0; i <= saveData.upgrades.cannShots.Length - 1; i++)
                {
                    if (saveData.upgrades.cannShots[i] == false)
                    {
                        saveData.upgrades.cannShots[i] = true;
                        saveData.difficulty += 10;
                        Destroy(gameObject);
                        break;
                    }                   
                }
            }
            else if (type == 1)
            {
                if (saveData.upgrades.canReloadSpeed == false)
                {
                    saveData.upgrades.canReloadSpeed = true;
                    saveData.difficulty += 10;
                    Destroy(gameObject);
                }
            }
            else if (type == 2)
            {
                if (saveData.upgrades.shotRadius == false)
                {
                    saveData.upgrades.shotRadius = true;
                    saveData.difficulty += 10;
                    Destroy(gameObject);
                }
            }
            else if (type == 3)
            {
                for (int i = 0; i <= saveData.upgrades.machSpeed.Length - 1; i++)
                {
                    if (saveData.upgrades.machSpeed[i] == false)
                    {
                        saveData.upgrades.machSpeed[i] = true;
                        saveData.difficulty += 10;
                        Destroy(gameObject);
                        break;
                    }
                }
            }
            else if (type == 4)
            {
                for (int i = 0; i <= saveData.upgrades.machAmmo.Length - 1; i++)
                {
                    if (saveData.upgrades.machAmmo[i] == false)
                    {
                        saveData.upgrades.machAmmo[i] = true;
                        saveData.difficulty += 10;
                        Destroy(gameObject);
                        break;
                    }
                }
            }
            else if (type == 5)
            {
                if (saveData.upgrades.machReloadSpeed == false)
                {
                    saveData.upgrades.machReloadSpeed = true;
                    saveData.difficulty += 10;
                    Destroy(gameObject);
                }
            }
            else if (type == 6)
            {
                for (int i = 0; i <= saveData.upgrades.dashAmount.Length - 1; i++)
                {
                    if (saveData.upgrades.dashAmount[i] == false)
                    {
                        saveData.upgrades.dashAmount[i] = true;
                        saveData.difficulty += 10;
                        Destroy(gameObject);
                        break;
                    }
                }
            }
            else if (type == 7)
            {
                if (saveData.upgrades.dashRechargeSpeed == false)
                {
                    saveData.upgrades.dashRechargeSpeed = true;
                    saveData.difficulty += 10;
                    Destroy(gameObject);
                }
            }
            else if (type == 8)
            {
                for (int i = 0; i <= saveData.upgrades.health.Length - 1; i++)
                {
                    if (saveData.upgrades.health[i] == false)
                    {
                        saveData.upgrades.health[i] = true;
                        saveData.difficulty += 10;
                        Destroy(gameObject);
                        break;
                    }
                }
            }            
        }
    }
}
