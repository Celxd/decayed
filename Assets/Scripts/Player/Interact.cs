using UnityEngine;

public class Interact : MonoBehaviour
{
    public bool canInteract = true; // Whether the object can currently be interacted with
    public Animator animator; // Reference to the Animator component

    private bool isInRange = false; // Whether the player is in range to interact

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    private void Update()
    {
        if (canInteract && isInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartAnimation();
        }
    }

    private void StartAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Interact"); // Trigger the "Interact" animation state
        }
    }
}
