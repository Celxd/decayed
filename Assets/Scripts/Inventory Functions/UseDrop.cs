using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseDrop : MonoBehaviour
{
    GameObject _player;
    Consumables _item;
    UseConsumables _use;
    Inventory _inventory;

    private void Awake()
    {
        _player = GameObject.Find("Player");
        _use = _player.GetComponent<UseConsumables>();
        _inventory = _player.GetComponent<Inventory>();
        _item = transform.parent.GetComponentInParent<InventoryItem>().currentItem;
    }

    public void OnUse()
    {
        Use(_item);
    }

    void Use(Consumables item)
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
    }

    public void OnDrop()
    {
        Debug.Log("drop");
        Drop(_item);
    }

    void Drop(Consumables item)
    {
        _inventory.RemoveItem(item);

        GameObject spawn = Instantiate(item.model, _player.transform.position + (_player.transform.forward * 2), _player.transform.rotation);
        Destroy(transform.parent.transform.parent.gameObject);
    }
}