using UnityEngine;

public class inventory : MonoBehaviour
{
    public GameObject Inventorypanel;
    bool isPaused = false;
    CursorLockMode previousLockState;
    bool previousCursorVisibility;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Inventorypanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            SaveCursorSettings();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Inventorypanel.SetActive(false);
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
