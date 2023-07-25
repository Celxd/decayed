using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100;
    public GameObject cam;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        cam.GetComponent<CinemachineBrain>().enabled = false;
        cam.GetComponent<BoxCollider>().enabled = true;
        cam.GetComponent<Rigidbody>().isKinematic = false;


        cam.transform.parent = null;
        Destroy(gameObject);
    }

}