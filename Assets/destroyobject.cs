using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyobject : MonoBehaviour
{
    public float destroyDelay = 5.0f; // Time in seconds before the object is destroyed

    private void Start()
    {
        // Call the DestroyObject method after 'destroyDelay' seconds
        Destroy(gameObject, destroyDelay);
    }
}
