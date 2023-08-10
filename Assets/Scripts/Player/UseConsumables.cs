using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseConsumables : MonoBehaviour
{
    public void UseItem(Consumables item)
    {
        switch(item.consumeType)
        {
            case ConsumeType.Ammo:
                return;
            case ConsumeType.Food:
                //add hunger
                break;
            case ConsumeType.Med:
                //add health
                break;
            case ConsumeType.Beverage:
                //add drink
                break;
        }
    }
}
