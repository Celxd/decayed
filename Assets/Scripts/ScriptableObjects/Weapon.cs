using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item
{
    public WeaponType weaponType;
    public WeaponCategory weaponCategory;
    
    [Header("Gun Stats")]
    public float fireRate;
    public float range;
    public int magSize;
    public int magCount;
    public float reloadTime;
    
    [Header("Gun Effects")]
    public GameObject gunModel;
    
    public bool reloading;
    public float currentAmmo;
}

public enum WeaponType {  Melee, Pistol, Rifle, Sniper, Shotgun, Submachine }
public enum WeaponCategory { Primary, Secondary, Melee }