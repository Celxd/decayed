using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;
    public float staminaDepletionRate = 20f;

    public Slider staminaBar;

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    private void Update()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            UpdateStaminaBar();
        }
    }

    public void DepleteStamina()
    {
        currentStamina -= staminaDepletionRate;
        if (currentStamina < 0f)
        {
            currentStamina = 0f;
        }
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        staminaBar.value = currentStamina / maxStamina;
    }
}
