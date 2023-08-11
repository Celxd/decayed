using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseConsumables : MonoBehaviour
{
    //Anggap aja class ini ada di player
    Health m_health;

    private void Awake()
    {
        m_health = GetComponent<Health>();
    }

    public void UseMedkit(Consumables med)
    {
        if (med.consumeType != ConsumeType.Med)
            return;

        if (m_health.CurrentHealth == m_health.MaxHealth)
            return;

        m_health.Heal(med.restorePoint);

        //remove object using inventory.RemoveItem(med)
        //but since we dont actually know where we're gonna put this I wont write it
        //Same with other functions
    }

    public void UseFood(Consumables food)
    {
        if (food.consumeType != ConsumeType.Food)
            return;

        m_health.AddHunger(food.restorePoint);
    }

    //yaudahla yah thirst juga sama
}
