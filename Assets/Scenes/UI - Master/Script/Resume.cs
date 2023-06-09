using UnityEngine;
using UnityEngine.UI;

public class Resume : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    bool isPaused = false;
    CursorLockMode previousLockState;
    bool previousCursorVisibility;

    void Start()
    {
        pauseButton.onClick.AddListener(TogglePause);
    }

    void Update()
    {
        // Optionally, you can still use the Escape key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            SaveCursorSettings();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            RestoreCursorSettings();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
