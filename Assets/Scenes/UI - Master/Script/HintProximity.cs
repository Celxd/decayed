using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HintProximity : MonoBehaviour
{
    public GameObject player;
    public float proximityDistance = 5f;
    public GameObject panel;
    public GameObject hidden;
    public PlayableDirector playableDirector;

    private bool isPlayerInRange = false;
    private bool isPanelActive = false;
    private bool isTimeFrozen = false;
    private Cinemachine.CinemachineBrain cinemachineBrain;

    private void Start()
    {
        panel.SetActive(false);
        hidden.SetActive(false); // Hide the 'hidden' game object initially
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cinemachineBrain = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        SceneManager.sceneUnloaded += OnSceneChange;
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneChange;
    }

    private void OnSceneChange(Scene scene)
    {
        ResumeTime();
        UnlockCursor();
        ResumeCameraMovement();
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
                isPanelActive = true;
                FreezeTime();
                LockCursor(); // Lock the cursor when the hidden object is shown
                PauseCameraMovement();
                StopPlayable();
            }
            else
            {
                UnlockCursor();
                panel.SetActive(false);
                hidden.SetActive(true);
                isPanelActive = true;
                playableDirector.Play();
                StartCoroutine(ShowHiddenDelayed());
            }
        }

        if (isPanelActive && Input.GetKeyDown(KeyCode.F))
        {
            hidden.SetActive(false);
            UnlockCursor(); // Unlock the cursor when hiding the hidden object
            ResumeTime();
            ResumeCameraMovement();
        }
    }

    private IEnumerator ShowHiddenDelayed()
    {
        yield return new WaitForSeconds(2f); // Adjust the delay duration as needed
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
