using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public GameObject Player;
    public Collider WaterCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            Health healthScript = Player.GetComponent<Health>();
            if (healthScript != null)
            {
                healthScript.TakeDamage(100f);
            }
        }
    }
}
