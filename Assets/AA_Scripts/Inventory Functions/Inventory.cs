using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;
    [SerializeField] Weapon hand;
    [SerializeField] List<Consumables> items;
    [SerializeField] InventorySlot[] slotsUI;
    [SerializeField] GameObject inventoryItemPrefab;

    PlayerShooting shoot;

    private void Start()
    {
        //InitVariables();
        weapons[2] = hand;

        shoot = GetComponent<PlayerShooting>();
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
        //string newItemID = newItem.ID;

        foreach (Consumables item in items)
        {
            if (item == null)
                continue;

            if (item.consumeType != newItem.consumeType)
                continue;

            if (newItem.consumeType == ConsumeType.Ammo)
            {
                if (item.ammoType != newItem.ammoType)
                    continue;
            }

            item.stack += newItem.stack;

            break;
        }
        Consumables newNewItem = Instantiate(newItem);
        items.Add(Instantiate(newNewItem));
        for (int i = 0; i < slotsUI.Length; i++)
        {
            InventorySlot slot = slotsUI[i];
            InventoryItem slotItem = slot.GetComponentInChildren<InventoryItem>();
            if (slotItem == null)
            {
                SpawnUIItem(newNewItem, slot);
                break;
            }
        }
        shoot.InitWeapon();
    }

    void SpawnUIItem(Consumables item, InventorySlot slot)
    {
        GameObject newUIItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newUIItem.GetComponent<InventoryItem>();
        inventoryItem.InitItem(item);
    }

    public void RemoveItem(Consumables currentConsum)
    {
        foreach (Consumables item in items)
        {
            if (item == null)
                continue;

            if (currentConsum.consumeType == ConsumeType.Ammo)
            {
                if (item.ammoType != currentConsum.ammoType)
                    continue;
            }

            if (item.stack - 1 <= 0)
                items.Remove(item);
            else
                item.stack -= 1;

            break;
        }

        for (int i = 0; i < slotsUI.Length; i++)
        {
            InventorySlot slot = slotsUI[i];
            InventoryItem slotItem = slot.GetComponentInChildren<InventoryItem>();

            if (slotItem == null)
                continue;


            if (slotItem.currentItem.consumeType != currentConsum.consumeType)
                continue;

            if (slotItem.currentItem.consumeType == ConsumeType.Ammo)
            {
                if (slotItem.currentItem.ammoType == currentConsum.ammoType)
                {
                    Destroy(slot.transform.GetChild(0).gameObject);
                    break;
                }
            }
            else
                Destroy(slot.transform.GetChild(0).gameObject);

            break;
        }

        shoot.InitWeapon();
    }

    public List<Consumables> GetAllItems()
    {
        return items;
    }

    public Consumables SearchItemByType(ConsumeType type)
    {
        foreach (Consumables item in items)
        {
            if (item.consumeType != type)
                continue;

            return item;
        }

        return null;
    }

    public Consumables SearchAmmo(AmmoType type)
    {
        foreach(Consumables item in items)
        {
            if (item.consumeType != ConsumeType.Ammo)
                continue;

            if (item.ammoType == type)
                return item;
            else
                continue;
        }
        return null;
    }
}
