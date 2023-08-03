using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    [SerializeField] Weapon hand;
    [SerializeField] Item[] items;

    private void Start()
    {
        //InitVariables();
        weapons[2] = hand;
    }

    public void AddWeapon(Weapon newWeapon)
    {
        int newItemIndex = (int)newWeapon.weaponCategory;
        
        if (weapons[newItemIndex] != null)
        {
            RemoveWeapon(newItemIndex);
        }

        weapons[newItemIndex] = Instantiate(newWeapon);
    }

    public void RemoveWeapon(int index)
    {
        weapons[index] = null;
    }

    public Weapon GetWeapon(int index)
    {
        return weapons[index];
    }

    private void InitVariables()
    {
        weapons = new Weapon[3];
    }
}
