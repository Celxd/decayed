using UnityEngine;

public class Limit : MonoBehaviour
{
    private Vector3 originalPosition;

    void Start()
    {
        // Store the player's original position at the start of the game
        originalPosition = transform.position;
    }

    void Update()
    {
        // Check if the player has fallen below the map's y-coordinate
        if (transform.position.y < -10.0f)
        {
            // Reset the player's position to the original position
            transform.position = originalPosition;
        }
    }
}
