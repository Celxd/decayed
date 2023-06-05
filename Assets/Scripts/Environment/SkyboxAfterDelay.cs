using UnityEngine;

public class SkyboxAfterDelay : MonoBehaviour
{
    public Material newSkybox;  // The new skybox material to be applied
    public float delay = 3f;    // Delay in seconds before changing the skybox

    private void Start()
    {
        Invoke("ChangeSkybox", delay);
    }

    private void ChangeSkybox()
    {
        RenderSettings.skybox = newSkybox;
    }
}
