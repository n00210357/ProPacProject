using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "upgrades", menuName = "upgrades")]
public class upgrades : ScriptableObject
{
    public bool[] cannShots = new bool[6];
    public bool canReloadSpeed;
    public bool shotRadius;

    public bool[] machSpeed  = new bool[6];
    public bool[] machAmmo  = new bool[3];
    public bool machReloadSpeed;

    public bool[] dashAmount  = new bool[3];
    public bool dashRechargeSpeed;

    public bool[] health = new bool[6];
}
