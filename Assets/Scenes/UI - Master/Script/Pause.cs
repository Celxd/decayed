using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public CinemachineVirtualCamera playerCamera;
    public GameObject panelParent; // Input field for the parent GameObject
    public GameObject player;
    private bool isPaused = false;
    private CursorLockMode previousLockState;
    private bool previousCursorVisibility;
    private float previousTimeScale;
    private bool isCameraPaused = false;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            SaveCursorSettings();
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;

            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
            }

            if (panelParent != null)
            {
                panelParent.SetActive(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = true;
            }

            if (playerCamera != null)
            {
                PauseCameraMovement();
            }
        }
        else
        {
            Time.timeScale = previousTimeScale;

            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }

            if (panelParent != null)
            {
             
                panelParent.SetActive(false);
            }

            RestoreCursorSettings();

            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = false;
            }

            if (playerCamera != null)
            {
                ResumeCameraMovement();
            }
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

    private void PauseCameraMovement()
    {
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
            isCameraPaused = true;
        }
    }

    private void ResumeCameraMovement()
    {
        if (playerCamera != null && isCameraPaused)
        {
            playerCamera.enabled = true;
            isCameraPaused = false;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isPaused)
        {
            Time.timeScale = 1f;
            RestoreCursorSettings();

            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = false;
            }

            if (playerCamera != null)
            {
                ResumeCameraMovement();
            }
        }
    }
}
