using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseConsumables : MonoBehaviour
{
    //Anggap aja class ini ada di player
    Health m_health;
    Inventory _inventory;

    private void Awake()
    {
        m_health = GetComponent<Health>();
        _inventory = GetComponent<Inventory>();
    }

    public void UseMedkit(Consumables med)
    {
        if (med.consumeType != ConsumeType.Med)
            return;

        if (m_health.CurrentHealth == m_health.MaxHealth)
            return;

        m_health.Heal(med.restorePoint);

        _inventory.RemoveItem(med);
    }

    public void UseFood(Consumables food)
    {
        if (food.consumeType != ConsumeType.Food)
            return;

        m_health.AddHunger(food.restorePoint);

        _inventory.RemoveItem(food);
    }

    public void UseDrink(Consumables food)
    {
        if (food.consumeType != ConsumeType.Beverage)
            return;

        m_health.AddThirst(food.restorePoint);

        _inventory.RemoveItem(food);
    }
}
