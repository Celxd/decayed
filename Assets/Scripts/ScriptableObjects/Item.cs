using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Info")]
    public string name;
    public Type type;
    public Sprite icon;

    public enum Type { Weapon, Ammo, Consumables }
}
