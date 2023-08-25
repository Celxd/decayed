using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingBar;
    public TMP_Text loadingText; // Reference to the TMP Text element

    private void Awake()
    {
        Time.timeScale = 1f; // Reset the timescale to its default value
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadSceneAsync(levelName));
    }

    private IEnumerator LoadSceneAsync(string levelName)
    {
        loadingPanel.SetActive(true); // Show the loading panel

        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            loadingBar.value = progress * 100f; // Convert progress to a scale of 0 to 100

            // Update the TMP Text with loading progress
            loadingText.text = $"Loading: {Mathf.Round(progress * 100f)}%";

            yield return null; // Wait for the next frame
        }

        loadingPanel.SetActive(false); // Hide the loading panel after loading
        loadingText.text = "Loading Complete"; // Optionally update the text after loading
    }
}
