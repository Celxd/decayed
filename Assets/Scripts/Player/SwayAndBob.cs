using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SwayAndBob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject weaponHolder;

    [Header("Settings")]
    public bool sway = true;
    public bool swayRotation = true;

    [Header("Sway Settings")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayRot;

    private PlayerInput playerInput;
    private InputAction action_Look;
    Vector2 lookInput;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        action_Look = playerInput.actions["Look"];
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(action_Look.ReadValue<Vector2>().ToString());
        GetInput();
    }

    void GetInput()
    {
        lookInput.x = action_Look.ReadValue<Vector2>().x;
        lookInput.y = action_Look.ReadValue<Vector2>().y;
    }

    void Sway()
    {
        if (sway == false) { swayPos = Vector3.zero; return; }

        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    void SwayRotation()
    {
        if (swayRotation == false) { swayRot = Vector3.zero; return; }

        Vector3 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

        swayRot = new Vector3(invertLook.y, invertLook.x, invertLook.z);
    }
}
