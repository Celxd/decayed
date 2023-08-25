using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class FastTravel : MonoBehaviour
{
    public GameObject player;
    public float proximityDistance = 5f;
    public GameObject panel;
    public PlayableDirector playableDirector;
    public Transform destinationPortal;

    private bool isPlayerInRange = false;
    private bool isPanelActive = false;
    private bool isTimeFrozen = false;
    private bool isTeleporting = false;
    private Cinemachine.CinemachineBrain cinemachineBrain;

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
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        isTeleporting = true;

        Vector3 destinationPosition = destinationPortal.position;
        Quaternion destinationRotation = destinationPortal.rotation;
        player.transform.position = destinationPosition;
        player.transform.rotation = destinationRotation;

        StartCoroutine(ResetTeleportFlag());
    }

    private IEnumerator ShowHiddenDelayed()
    {
        yield return new WaitForSeconds(2f);
        // Code to show hidden elements
    }

    private IEnumerator ResetTeleportFlag()
    {
        yield return new WaitForSeconds(0.5f);
        isTeleporting = false;
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

    private void PauseCameraMovement()
    {
        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = false;
        }
    }

    private void ResumeCameraMovement()
    {
        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = true;
        }
    }

    private void StopPlayable()
    {
        if (playableDirector != null && playableDirector.state == PlayState.Playing)
        {
            playableDirector.Stop();
        }
    }
}
