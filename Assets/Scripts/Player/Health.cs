using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] float health = 100;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject healthbar;
    [SerializeField] GameObject diepanel;
    [SerializeField] GameObject holder;
    

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

        Destroy(holder);
        cam.transform.parent = null;

        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(DeathUI());
        
    }

    IEnumerator DeathUI()
    {
        yield return new WaitForSeconds(3);

        diepanel.SetActive(true);
        diepanel.GetComponent<Animator>().SetBool("dead", true);

        Destroy(gameObject);
    }

}