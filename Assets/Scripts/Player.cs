using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5.0f; // adjust this value to change the movement speed
    public float jumpForce = 7.0f; // adjust this value to change the jump force

    private bool isGrounded; // flag to check if the player is on the ground

    void Update()
    {
        // Get the input from the W, A, S, D keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the movement vector based on the input and the speed
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * speed * Time.deltaTime;

        // Apply the movement to the player's position
        transform.position += movement;

        // Check if the player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // Check if the player pressed the jump key and is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Add a vertical force to make the player jump
            GetComponent<Rigidbody>().velocity = new Vector3(0, jumpForce, 0);
        }
    }
}
