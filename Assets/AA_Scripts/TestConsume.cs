using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConsume : MonoBehaviour
{
    public Consumables item;
    public UseConsumables _use;

    // Start is called before the first frame update
    void Start()
    {
        _use = GetComponent<UseConsumables>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _use.UseMedkit(item);
        }
    }
}
