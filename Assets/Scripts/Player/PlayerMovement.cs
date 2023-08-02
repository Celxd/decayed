using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    [HideInInspector] public PlayerInput playerInput;
    private Vector3 playerVelocity;
    private Transform camTransform;
    private bool groundedPlayer;
    private bool isRunning;
    private bool isCrouching = false;

    [Header("Settings")]
    [SerializeField] float playerSpeed = 2.0f;
    [SerializeField] float jumpHeight = 1.0f;
    [SerializeField] float gravityValue = -9.81f;
    [SerializeField] float rotationSpeed = 5f;

    [Header("Crouch Settings")]
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float crouchTransitionSpeed = 5f;

    float height;
    float currentSpeed;
    float currentHeight;
    InputAction action_move;
    InputAction action_jump;
    InputAction action_run;
    InputAction action_crouch;

    public float maxStamina = 100f;
    public float currentStamina = 100f; // Starting stamina count is 100
    public float staminaRegenRate = 10f;
    public float staminaDepletionRate = 20f;
    public Slider staminaSlider;

    public float maxThirst = 100f;
    public float currentThirst = 100f; // Starting thirst count is 100
    public float thirstDepletionRate = 5f;
    public float thirstThresholdForRunning = 30f;
    public Slider thirstSlider;

    private bool isDepletingStamina = false;

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
        action_run.canceled += ctx =>
        {
            isRunning = false;
            isDepletingStamina = false;
        };

        action_crouch.started += ctx => isCrouching = true;
        action_crouch.canceled += ctx => isCrouching = false;

        height = currentHeight = controller.height;
    }

    private void Start()
    {
        Time.timeScale = 1;
        currentStamina = maxStamina; // Set stamina count to 100
        currentThirst = maxThirst; // Set thirst count to 100
        UpdateStaminaUI();
        UpdateThirstUI();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (isCrouching)
            Crouch();
        else
            CrouchEnd();

        currentSpeed = playerSpeed;
        if (isRunning && currentStamina > 0 && currentThirst > 0)
        {
            currentSpeed *= 2;
            isDepletingStamina = true;
        }

        // Read input and do movement
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

        // Rotate player to camera
        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
        transform.rotation = rotation;

        // Stamina management
        if (isRunning && isDepletingStamina)
        {
            // Deplete stamina while running
            currentStamina -= staminaDepletionRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
        else
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }

        // Thirst management
        if (currentThirst > 0f)
        {
            currentThirst -= thirstDepletionRate * Time.deltaTime;
            currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);
            if (currentThirst <= thirstThresholdForRunning)
            {
                isRunning = false;
                isDepletingStamina = true;
            }
        }
        else
        {
            isRunning = false;
            isDepletingStamina = true;
        }

        // Update the UI sliders
        UpdateStaminaUI();
        UpdateThirstUI();
    }

    void Crouch()
    {
        // Check if there is enough stamina and thirst to crouch while running
        if ((isRunning && currentStamina < staminaDepletionRate * Time.deltaTime) || currentThirst <= 0f)
        {
            isRunning = false;
            isDepletingStamina = true;
        }

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

        // If not running, stop depleting stamina
        if (!isRunning)
        {
            isDepletingStamina = false;
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina / maxStamina;
        }
    }

    void UpdateThirstUI()
    {
        if (thirstSlider != null)
        {
            thirstSlider.value = currentThirst / maxThirst;
        }
    }
}
