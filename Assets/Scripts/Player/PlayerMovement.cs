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
    private bool isCrouching = false;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = 5f;
    private float height;
    private float currentSpeed;
    private float currentHeight;
    private InputAction action_move;
    private InputAction action_jump;
    private InputAction action_run;
    private InputAction action_crouch;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = gameObject.GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        camTransform = Camera.main.transform;

        action_move = playerInput.actions["Move"];
        action_jump = playerInput.actions["Jump"];
        action_run = playerInput.actions["Run"];
        action_crouch = playerInput.actions["Crouch"];

        action_run.started += ctx => isRunning = true;
        action_run.canceled += ctx => isRunning = false;

        action_crouch.started += ctx => isCrouching = true;
        action_crouch.canceled += ctx => isCrouching = false;

        height = currentHeight = controller.height;
    }
    
    void Update()
    {
        //reset velocity
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        //crouching
        if (isCrouching)
            Crouch();
        else
            CrouchEnd();

        //set running speed
        currentSpeed = playerSpeed;
        if (isRunning)
            currentSpeed *= 2;

        //read input and do movement
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

        //rotate player to camera
        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
        transform.rotation = rotation;
    }
    
    void Crouch()
    {
        float crouchDelta = Time.deltaTime * crouchTransitionSpeed;
        currentHeight = Mathf.Lerp(currentHeight, crouchHeight, crouchDelta);
        controller.height = currentHeight;
        currentSpeed /= 4;
    }

    void CrouchEnd()
    {
        float heightTarget = height;
        Vector3 castOrigin = transform.position + new Vector3(0, height / 2, 0);
        if (Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hit, 0.2f))
        {
            float distanceToCeiling = hit.point.y - castOrigin.y;
            heightTarget = Mathf.Max(currentHeight + distanceToCeiling - 0.1f, crouchHeight);
        }
        
        float crouchDelta = Time.deltaTime * crouchTransitionSpeed;
        currentHeight = Mathf.Lerp(currentHeight, heightTarget, crouchDelta);
        controller.height = currentHeight;
        currentSpeed = playerSpeed;
    }
}
