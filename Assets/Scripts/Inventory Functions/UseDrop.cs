using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseDrop : MonoBehaviour
{
    GameObject _player;
    Consumables _item;
    UseConsumables _use;
    Inventory _inventory;
    GameObject _itemUI;

    private void Awake()
    {
        _player = GameObject.Find("Player");
        _use = _player.GetComponent<UseConsumables>();
        _inventory = _player.GetComponent<Inventory>();
        _itemUI = transform.parent.transform.parent.gameObject;
        _item = _itemUI.GetComponent<InventoryItem>().currentItem;
    }

    public void OnUse()
    {
        Use(_item, _itemUI);
    }

    void Use(Consumables item, GameObject itemSlot)
    {
        if (item.consumeType == ConsumeType.Ammo)
            return;

        switch(item.consumeType)
        {
            case ConsumeType.Med:
                _use.UseMedkit(item);
                break;
            case ConsumeType.Food:
                _use.UseFood(item);
                break;
            case ConsumeType.Beverage:
                _use.UseDrink(item);
                break;
        }

        _inventory.RemoveItem(item);
        Destroy(itemSlot);
    }

    public void OnDrop()
    {
        Drop(_item, _itemUI);
    }

    void Drop(Consumables item, GameObject itemSlot)
    {
        _inventory.RemoveItem(item);

        GameObject spawn = Instantiate(item.model, _player.transform.position + (_player.transform.forward * 2), _player.transform.rotation);
        Destroy(itemSlot);
    }
}