using UnityEngine;

public class Animals : MonoBehaviour
{
    public float minRange = 1f; // minimum distance the animal moves before changing direction
    public float maxRange = 5f; // maximum distance the animal moves before changing direction
    public float moveSpeed = 5f; // speed at which the animal moves
    public float rotationSpeed = 5f; // speed at which the animal rotates towards new direction

    private Vector3 randomDirection; // direction the animal will move in
    private float currentRange; // current distance the animal has moved
    private Vector3 startingPosition; // starting position of the animal

    void Start()
    {
        // save the starting position of the animal
        startingPosition = transform.position;

        // set a random initial direction for the animal
        randomDirection = GetRandomDirection();
    }

    void Update()
    {
        // calculate the target rotation towards the new direction
        Quaternion targetRotation = Quaternion.LookRotation(randomDirection, Vector3.up);

        // rotate towards the new direction at a controlled speed
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // move the animal in its current direction
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // update the current distance moved
        currentRange += moveSpeed * Time.deltaTime;

        // if the animal has moved far enough, choose a new random direction
        if (currentRange >= maxRange)
        {
            // reset the current range and choose a new random direction
            currentRange = 0f;
            randomDirection = GetRandomDirection();
        }
    }

    private Vector3 GetRandomDirection()
    {
        // generate a random direction in the x and z plane
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        float x = Mathf.Cos(randomAngle);
        float z = Mathf.Sin(randomAngle);

        // create a vector in the chosen direction with a random magnitude within the specified range
        float randomMagnitude = Random.Range(minRange, maxRange);
        Vector3 randomDirection = new Vector3(x, 0f, z) * randomMagnitude;

        // return the random direction
        return randomDirection;
    }

    // reset the animal to its starting position and choose a new random direction
    public void ResetAnimal()
    {
        transform.position = startingPosition;
        currentRange = 0f;
        randomDirection = GetRandomDirection();
    }
}
