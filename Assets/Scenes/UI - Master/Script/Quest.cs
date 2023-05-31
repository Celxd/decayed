using UnityEngine;

public class Quest : MonoBehaviour
{
    public GameObject Questpanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isPaused = Time.timeScale == 0;
        Questpanel.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1 : 0;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void HidePausePanel()
    {
        Questpanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
