using UnityEngine;

public class RotateDirectionalLight : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // Degrees per second
    private Transform directionalLightTransform;

    private void Start()
    {
        // Find the directional light's transform
        directionalLightTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        // Calculate the rotation angle for this frame
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // Rotate the light around the Z-axis
        directionalLightTransform.Rotate(Vector3.right, rotationAngle);
    }
}
