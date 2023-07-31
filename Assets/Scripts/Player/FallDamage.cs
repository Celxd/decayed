using UnityEngine;

public class FallDamage : MonoBehaviour
{
    public float fallHeightThreshold = 5f;
    public float maxFallDamage = 50f;
    private CharacterController characterController;
    public float fallDamageMultiplier = 2f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {

        if (characterController.velocity.y < 0f && Mathf.Abs(characterController.velocity.y) > fallHeightThreshold)
        {
            float fallHeight = Mathf.Abs(characterController.velocity.y) - fallHeightThreshold;
            float damage = fallHeight * fallDamageMultiplier;
            damage = Mathf.Clamp(damage, 0f, maxFallDamage);
        }
    }
}