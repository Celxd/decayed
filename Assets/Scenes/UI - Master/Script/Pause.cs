using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    bool isPaused = false;
    CursorLockMode previousLockState;
    bool previousCursorVisibility;
    float previousTimeScale;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            SaveCursorSettings();
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = previousTimeScale;
            RestoreCursorSettings();
        }
    }

    private void SaveCursorSettings()
    {
        previousLockState = Cursor.lockState;
        previousCursorVisibility = Cursor.visible;
    }

    private void RestoreCursorSettings()
    {
        Cursor.lockState = previousLockState;
        Cursor.visible = previousCursorVisibility;
    }
}
