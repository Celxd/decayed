using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    [SerializeField] Weapon hand;
    [SerializeField] List<Consumables> items;

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

    public void AddItem(Consumables newItem)
    {
        string newItemID = newItem.ID;

        foreach (Consumables item in items)
        {
            if (item.ID != newItemID)
                break;

            item.stack += newItem.stack;
            return;
        }
        items.Add(newItem);
    }

    public void RemoveItem(Consumables currentConsum)
    {
        foreach (Consumables item in items)
        {
            if (item.ID != currentConsum.ID)
                break;

            if (item.stack - 1 <= 0)
                items.Remove(item);
            else
                item.stack -= 1;

            return;
        }
    }

    private void InitVariables()
    {
        weapons = new Weapon[3];
    }
}
