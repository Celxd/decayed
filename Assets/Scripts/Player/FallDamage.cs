using UnityEngine;

public class FallDamage : MonoBehaviour
{
    public float fallHeightThreshold = 5f;
    public float maxFallDamage = 50f;
    public float fallDamageMultiplier = 5f;

    private CharacterController characterController;
    private Health healthScript;
    private bool isFalling = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        healthScript = GetComponent<Health>();
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            if (isFalling)
            {
                isFalling = false;

                float fallHeight = Mathf.Abs(characterController.velocity.y) - fallHeightThreshold;
                float damage = fallHeight * fallDamageMultiplier;
                damage = Mathf.Clamp(damage, 0f, maxFallDamage);

                if (healthScript != null)
                {
                    healthScript.TakeDamage(damage);
                }
            }
        }
        else
        {
            isFalling = true;
        }
    }
}
