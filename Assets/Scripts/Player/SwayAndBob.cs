using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SwayAndBob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject weaponHolder;
    PlayerInput playerInput;
    InputAction action_Look;
    InputAction action_Move;
    CharacterController characterController;
    PlayerShooting playerShoot;

    [Header("Settings")]
    public bool sway = true;
    public bool swayRotation = true;
    public bool bobOffset = true;
    public bool bobSway = true;
    public float smooth = 10f;
    public float smoothRot = 12f;

    [Header("Sway Settings")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayRot;

    [Header("Bobbing Settings")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }
    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;

    [Header("Bobbing Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;
    
    Vector3 bobPosition;
    Vector3 defaultPos;
    Vector3 defaultRot;
    Vector2 lookInput;
    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerShoot = GetComponent<PlayerShooting>();
        action_Look = playerInput.actions["Look"];
        action_Move = playerInput.actions["Move"];

        defaultPos = weaponHolder.transform.localPosition;
        defaultRot = weaponHolder.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponHolder == null)
            return;

        GetInput();
        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        //Apply value
        CompositePositionRotation();
    }

    void GetInput()
    {
        lookInput.x = action_Look.ReadValue<Vector2>().x;
        lookInput.y = action_Look.ReadValue<Vector2>().y;

        moveInput = action_Move.ReadValue<Vector2>();
    }

    void Sway()
    {
        if (sway == false) { swayPos = defaultPos; return; }

        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    void SwayRotation()
    {
        if (swayRotation == false) { swayRot = defaultRot; return; }

        Vector3 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

        swayRot = new Vector3(invertLook.y, invertLook.x, invertLook.z);
    }

    void CompositePositionRotation()
    {
        if (playerShoot)
        {
            weaponHolder.transform.localPosition = defaultPos;
            weaponHolder.transform.localRotation = Quaternion.Euler(defaultRot);
            return;
        }
        //Pos
        weaponHolder.transform.localPosition = Vector3.Lerp(weaponHolder.transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);

        //Rot
        weaponHolder.transform.localRotation = Quaternion.Slerp(weaponHolder.transform.localRotation, Quaternion.Euler(swayRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    void BobOffset()
    {
        speedCurve += Time.deltaTime * (characterController.isGrounded ? characterController.velocity.magnitude : 1f) + 0.01f;

        if (bobOffset == false) { bobPosition = Vector3.zero; return; }
        
        bobPosition.x = (curveCos * bobLimit.x * (characterController.isGrounded ? 1 : 0)) - (moveInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (characterController.velocity.y * travelLimit.y);
        bobPosition.z = - (moveInput.y * travelLimit.z);
    }

    void BobRotation()
    {
        if (bobSway == false) { bobEulerRotation = Vector3.zero; return; }

        bobEulerRotation.x = (moveInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (moveInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (moveInput != Vector2.zero ? multiplier.z * curveCos * moveInput.x : 0);
    }
}
