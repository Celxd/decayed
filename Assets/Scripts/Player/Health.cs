using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject cam;
    [SerializeField] GameObject diepanel;
    [SerializeField] GameObject holder;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider hungerSlider;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float maxHunger = 100;
    private float currentHealth;
    private float currentHunger;

    public float hungerRate = 1f;


    private void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        UpdateHealthUI();
        UpdateHungerUI();

    }

    private void DecreaseHunger()
    {
        currentHunger -= hungerRate;
        UpdateHungerUI();

        if (currentHunger <= 0f)
        {
            TakeDamage(5f);
            currentHunger = 0f;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();

        UpdateHealthUI();
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

    private void UpdateHealthUI()
    {
        healthSlider.value = currentHealth / maxHealth;
    }
    private void UpdateHungerUI()
    {
        hungerSlider.value = currentHunger / maxHealth;
    }
}
