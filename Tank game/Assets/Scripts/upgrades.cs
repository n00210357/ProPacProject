using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "upgrades", menuName = "upgrades")]
public class upgrades : ScriptableObject
{
    //cannon upgrades
    public bool[] cannShots = new bool[6];
    public bool canReloadSpeed;
    public bool shotRadius;

    //machine gun upgrades
    public bool[] machSpeed  = new bool[3];
    public bool[] machAmmo  = new bool[4];
    public bool machReloadSpeed;

    //dash upgrades
    public bool[] dashAmount  = new bool[3];
    public bool dashRechargeSpeed;

    //health upgrades
    public bool[] health = new bool[6];
}
