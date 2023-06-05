using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAfterDelay : MonoBehaviour
{
    public string sceneName; // Name of the scene to be loaded
    public float delay = 5f; // Delay in seconds before changing the scene

    private float timer = 0f;
    private bool sceneChangeStarted = false;

    private void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if the delay has passed and scene change has not started yet
        if (timer >= delay && !sceneChangeStarted)
        {
            sceneChangeStarted = true;

            // Call the function to change the scene
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
