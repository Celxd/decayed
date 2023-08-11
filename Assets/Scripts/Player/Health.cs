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
    [HideInInspector] public float MaxHealth { get { return maxHealth; } }
    [SerializeField] float maxHunger = 100;
    float currentHealth;
    public float CurrentHealth { get { return currentHealth; } }
    float currentHunger;

    public float hungerRate = 1f;

    private void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        UpdateHealthUI();
        UpdateHungerUI();
    }

    private void FixedUpdate()
    {
        DecreaseHunger();
    }

    private void DecreaseHunger()
    {
        currentHunger -= hungerRate * Time.fixedDeltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);
        UpdateHungerUI();

        if (currentHunger <= 0f)
        {
            TakeDamage(5f);
            currentHunger = 0f;
        }
    }
    public void AddHunger(float food)
    {
        currentHunger += food;
        currentHunger = Mathf.Clamp(currentHunger, 0f, maxHunger);

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0)
            Die();

        UpdateHealthUI();
    }

    public void Heal(float med)
    {
        currentHealth += med;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

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
 
        healthSlider.value = currentHealth;
    }

    private void UpdateHungerUI()
    {
        hungerSlider.value = currentHunger;
    }
}
