using UnityEngine;
using UnityEngine.UI;

public class HintProximity : MonoBehaviour
{
    public GameObject player;
    public float proximityDistance = 5f;
    public GameObject panel;
    public GameObject hidden;

    private bool isPlayerInRange = false;
    private bool isPanelActive = false;
    private bool isTimeFrozen = false;

    private void Start()
    {
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= proximityDistance && !isPlayerInRange)
        {
            isPlayerInRange = true;
            panel.SetActive(true);
        }
        else if (distance > proximityDistance && isPlayerInRange)
        {
            isPlayerInRange = false;
            panel.SetActive(false);
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (isPanelActive)
            {
                panel.SetActive(false);
                hidden.SetActive(true);
                isPanelActive = false;
                FreezeTime();
                UnlockCursor();
            }
            else
            {
                panel.SetActive(true);
                hidden.SetActive(false);
                isPanelActive = true;
                
            }
        }

        if (isPanelActive && Input.GetKeyDown(KeyCode.F))
        {
            LockCursor();
            ResumeTime();
        }
    }






    private void FreezeTime()
    {
        if (!isTimeFrozen)
        {
            Time.timeScale = 0f;
            isTimeFrozen = true;
        }
    }

    private void ResumeTime()
    {
        if (isTimeFrozen)
        {
            Time.timeScale = 1f;
            isTimeFrozen = false;
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
