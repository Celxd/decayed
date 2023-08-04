using UnityEngine;

public class Crow : MonoBehaviour
{
    public float flyAwayDistance = 10f;
    public float flyAwaySpeed = 5f;
    public float flyAwayDuration = 3f;
    private Vector3 initialPosition;
    private bool isFlyingAway = false;
    private float flyAwayTimer = 0f;
    private Vector3 velocity; // New variable to store bird's velocity
    public LayerMask Player;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isFlyingAway && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < flyAwayDistance)
        {
            isFlyingAway = true;
            // Generate a random direction and distance from the initial position
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            float randomDistance = Random.Range(0f, flyAwayDistance);
            Vector3 targetPosition = initialPosition + randomDirection * randomDistance;
            // Set the bird's rotation to face the target position
            transform.LookAt(targetPosition);
            flyAwayTimer = 0f;
        }

        if (isFlyingAway)
        {
            // Move the bird towards the target position
            velocity = transform.forward * flyAwaySpeed;
            transform.position += velocity * Time.deltaTime;
            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                isFlyingAway = false;
                flyAwayTimer = 0f;
                transform.position = initialPosition; // Reset position to initial position
            }
            else if (flyAwayTimer > flyAwayDuration)
            {
                isFlyingAway = false;
                flyAwayTimer = 0f;
            }
            else
            {
                flyAwayTimer += Time.deltaTime;
            }
        }
    }
}
