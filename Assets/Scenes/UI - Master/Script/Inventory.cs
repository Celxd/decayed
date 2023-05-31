using UnityEngine;

public class inventory : MonoBehaviour
{
    public GameObject InventoryPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isPaused = Time.timeScale == 0;
        InventoryPanel.SetActive(!isPaused);
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
        InventoryPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
