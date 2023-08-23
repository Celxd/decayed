using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    [HideInInspector] public PlayerInput playerInput;
    Vector3 playerVelocity;
    Transform camTransform;
    bool groundedPlayer;
    bool isRunning;
    bool isCrouching = false;

    PlayerShooting _shootingComponent;

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

    //---------------------------------------------------------
    public float stamina = 1.0f;
    public float maxStamina = 1.0f;

    //---------------------------------------------------------
    //private float staminaRegenTimer = 0.0f;

    //---------------------------------------------------------
    const float StaminaDecreasePerFrame = 10.0f;
    const float StaminaIncreasePerFrame = 10.0f;
    const float StaminaTimeToRegen = 3.0f;
    public Slider staminaSlider;


    public float maxThirst = 100f;
    public float currentThirst = 100f; // Starting thirst count is 100
    public float thirstDepletionRate = 5f;
    public float thirstThresholdForRunning = 30f;
    public Slider thirstSlider;

    bool isDepletingStamina = false;
    bool isMoving;
    float originalFOV;
    [SerializeField] float fovIncreaseAmount = 10f;
    [SerializeField] float fovLerpSpeed = 5f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = gameObject.GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        camTransform = Camera.main.transform;
        _shootingComponent = GetComponent<PlayerShooting>();

        action_move = playerInput.actions["Move"];
        action_jump = playerInput.actions["Jump"];
        action_run = playerInput.actions["Run"];
        action_crouch = playerInput.actions["Crouch"];

        action_run.started += ctx => isRunning = true;
        action_run.canceled += ctx => isRunning = false;

        action_move.started += ctx => isMoving = true;
        action_move.canceled += ctx => isMoving = false;

        action_crouch.started += ctx => isCrouching = true;
        action_crouch.canceled += ctx => isCrouching = false;

        height = currentHeight = controller.height;

        stamina = maxStamina;
        originalFOV = _shootingComponent.vcam.m_Lens.FieldOfView;
    }

    private void Start()
    {
        Time.timeScale = 1;
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
        if (isRunning)
        {
            if (stamina > 0f)
            {
                currentSpeed *= 2;
                if (isMoving)
                    stamina = Mathf.Clamp(stamina - (StaminaDecreasePerFrame * Time.deltaTime), 0.0f, maxStamina);

                // Print the field of view for debugging
                Debug.Log("Current FOV: " + Camera.main.fieldOfView);

                // Increase FOV when running
                _shootingComponent.vcam.m_Lens.FieldOfView = Mathf.Lerp(_shootingComponent.vcam.m_Lens.FieldOfView, originalFOV + fovIncreaseAmount, Time.deltaTime * fovLerpSpeed);
                Debug.Log("New FOV: " + Camera.main.fieldOfView);
            }
        }
        else if (stamina < maxStamina && !isRunning)
        {
            stamina = Mathf.Clamp(stamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, maxStamina);
            // Reset FOV to the original value when not running
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, originalFOV, Time.deltaTime * fovLerpSpeed);
        }

        UpdateStaminaUI();

        Vector2 input = action_move.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * camTransform.right.normalized + move.z * camTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * currentSpeed);

        if (action_jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
        transform.rotation = rotation;

        // Thirst management
        if (currentThirst > 0f)
        {
            float thirstDepletionRateMultiplier = isRunning && stamina <= thirstThresholdForRunning ? 2f : 1f;
            currentThirst -= thirstDepletionRate * thirstDepletionRateMultiplier * Time.deltaTime;
            currentThirst = Mathf.Clamp(currentThirst, 0f, maxThirst);
            UpdateThirstUI();

            if (currentThirst <= 0f)
            {
                isRunning = false;
                isDepletingStamina = true;
                UpdateThirstUI();
            }
        }
    }

    void Crouch()
    {
        // Check if there is enough stamina and thirst to crouch while running
        if ((isRunning && stamina < StaminaDecreasePerFrame * Time.deltaTime) || currentThirst <= 0f)
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
            staminaSlider.value = stamina;
        }
    }

    void UpdateThirstUI()
    {
        if (thirstSlider != null)
        {
            thirstSlider.value = currentThirst;
        }
    }
}
