using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCycle : MonoBehaviour
{
    public HDAdditionalLightData directionalLight;
    public Material skyMaterial;
    public Gradient skyColorGradient;
    public float dayCycleDuration = 10f;

    private float m_timeOfDay = 0f;

    private void Update()
    {
        // Update the time of day based on the real time
        m_timeOfDay += Time.deltaTime / dayCycleDuration;
        if (m_timeOfDay > 1f)
            m_timeOfDay = 0f;

        // Calculate the rotation of the directional light
        float sunAngle = m_timeOfDay * 360f;
        directionalLight.transform.localRotation = Quaternion.Euler(sunAngle, 0f, 0f);

        // Update the sky material color based on the time of day
        skyMaterial.SetColor("_SkyTint", skyColorGradient.Evaluate(m_timeOfDay));
    }
}
