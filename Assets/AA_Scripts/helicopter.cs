using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Helicopter : MonoBehaviour
{
    public GameObject player;
    public float proximityDistance = 5f;
    public GameObject panel;
    public GameObject hidden;

    private bool isPlayerInRange = false;
    private bool isPanelActive = false;
    private bool isTimeFrozen = false;





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


        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (isPanelActive)
            {
                hidden.SetActive(false);
                isPanelActive = false;
                LockCursor();
            }
            else
            {
                panel.SetActive(false);
                isPanelActive = true;
                UnlockCursor();
                StartCoroutine(ShowHiddenDelayed());
                ResumeTime();
  
            }
        }

    }

    private IEnumerator ShowHiddenDelayed()
    {
        yield return new WaitForSeconds(0f);
        hidden.SetActive(true);
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
