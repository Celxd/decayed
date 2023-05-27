using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;

    private void Start()
    {
        InitVariables();
    }

    public void AddItem(Weapon newWeapon)
    {
        int newItemIndex = (int)newWeapon.weaponCategory;
        
        if (weapons[newItemIndex] != null)
        {
            RemoveItem(newItemIndex);
        }
        
        weapons[newItemIndex] = newWeapon;
    }

    public void RemoveItem(int index)
    {
        weapons[index] = null;
    }

    public Weapon GetItem(int index)
    {
        return weapons[index];
    }

    private void InitVariables()
    {
        weapons = new Weapon[3];
    }
}
