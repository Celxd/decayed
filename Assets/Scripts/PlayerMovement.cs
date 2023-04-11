using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private Transform camTransform;
    private bool groundedPlayer;
    private bool isRunning;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    private InputAction action_move;
    private InputAction action_jump;
    private InputAction action_run_start;
    private InputAction action_run_end;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = gameObject.GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        camTransform = Camera.main.transform;

        action_move = playerInput.actions["Move"];
        action_jump = playerInput.actions["Jump"];
        action_run_start = playerInput.actions["RunStart"];
        action_run_end = playerInput.actions["RunEnd"];

        action_run_start.performed += ctx => isRunning = true;
        action_run_start.canceled += ctx => isRunning = false;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        float currentSpeed = playerSpeed;
        if (isRunning)
            currentSpeed *= 2;

        Vector2 input = action_move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * camTransform.right.normalized + move.z * camTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * currentSpeed);

        // Changes the height position of the player..
        if (action_jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
        transform.rotation = rotation;
    }
}
