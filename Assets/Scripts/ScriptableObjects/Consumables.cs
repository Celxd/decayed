using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumeable")]
public class Consumables : Item
{
    [Header("Type and Restore Point")]
    public ConsumeType consumeType;
    public float restorePoint;

    public enum ConsumeType { Food, Beverage, Med}
}
