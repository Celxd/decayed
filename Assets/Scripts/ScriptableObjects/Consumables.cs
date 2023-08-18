using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumeable")]
public class Consumables : Item
{
    [Header("Additional Info")]
    public float stack;
    public GameObject model;

    [Header("Type and Restore Point")]
    public ConsumeType consumeType;
    public float restorePoint;

    [Header("Ammo type (Don't change unless it's ammo)")]
    public AmmoType ammoType;

    protected override void OnValidate()
    {
        base.OnValidate();
        type = Type.Consumables;

        if (consumeType != ConsumeType.Ammo)
            ammoType = AmmoType.Non;
    }
}

public enum ConsumeType { Ammo, Food, Beverage, Med }
public enum AmmoType { Non, Pistol, Rifle, Sniper, Shotgun, Submachine }