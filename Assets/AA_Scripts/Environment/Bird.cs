using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float turnSpeed = 2f;
    public float changeDirectionTime = 3f;
    public float flightAltitude = 5f; 

    private float timer = 0f;
    private Vector3 targetDirection;

    void Start()
    {
        targetDirection = GetRandomDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > changeDirectionTime)
        {
            targetDirection = GetRandomDirection();
            timer = 0f;
        }

        // Calculate the bird's forward direction in the x-z plane
        Vector3 forwardDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

        // Calculate the target position based on the forward direction and the desired altitude
        Vector3 targetPosition = transform.position + forwardDirection * moveSpeed * Time.deltaTime + Vector3.up * (flightAltitude - transform.position.y);

        // Move the bird towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotate the bird towards the target direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), turnSpeed * Time.deltaTime);
    }

    Vector3 GetRandomDirection()
    {
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);

        return new Vector3(x, 0, z).normalized;
    }
    public void Die()
    {
        Destroy(gameObject);
    }

}
