using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isAlive = true;

    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;

        // Set the initial health slider value
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive)
            return;

        currentHealth -= damage;

        // Update the health slider value
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isAlive = false;
        // Play death animation, remove object, etc.
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        // Clamp the current health to the max health
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        // Update the health slider value
        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    public void Damage(int damage)
    {
        if (!isAlive)
            return;

        currentHealth -= damage;

        // Update the health slider value
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            Die();

        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
    }



}
