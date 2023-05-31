using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowController : MonoBehaviour
{
    // Reference to the player object
    public GameObject player;

    // Speed of the crow
    public float speed = 5f;

    // Distance from the player at which the crow will start following them
    public float followDistance = 10f;

    // Distance from the player at which the crow will stop following them
    public float stopDistance = 5f;

    // Distance from the player at which the crow will fly away
    public float flyAwayDistance = 2f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the crow and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the player is within the follow distance
        if (distanceToPlayer < followDistance)
        {
            // If the player is too close, fly away
            if (distanceToPlayer < flyAwayDistance)
            {
                // Calculate the direction away from the player
                Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

                // Move the crow away from the player
                transform.Translate(directionAwayFromPlayer * speed * Time.deltaTime, Space.World);

                // Rotate the crow to face away from the player
                transform.LookAt(transform.position - directionAwayFromPlayer);
            }
            else
            {
                // Calculate the direction to the player
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

                // Move the crow towards the player
                transform.Translate(directionToPlayer * speed * Time.deltaTime, Space.World);

                // Rotate the crow to face the player
                transform.LookAt(player.transform);
            }
        }
        // If the player is within the stop distance
        if (distanceToPlayer < stopDistance)
        {
            // Stop moving the crow
            speed = 0;
        }
        else
        {
            // Resume moving the crow
            speed = 5f;
        }

        // If the player is within the fly away distance, fly away
        if (distanceToPlayer < flyAwayDistance)
        {
            // Calculate the direction away from the player
            Vector3 directionAwayFromPlayer = (transform.position - player.transform.position).normalized;

            // Move the crow away from the player
            transform.Translate(directionAwayFromPlayer * speed * Time.deltaTime, Space.World);

            // Rotate the crow to face away from the player
            transform.LookAt(transform.position - directionAwayFromPlayer);
        }
    }
}
