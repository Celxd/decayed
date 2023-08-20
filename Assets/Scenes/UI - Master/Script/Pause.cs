using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject _player;
    CinemachineBrain _cam;
    bool isPaused = false;
    Quaternion _rot;

    private void Awake()
    {
        _player = GameObject.Find("Player");
        _cam = _player.GetComponentInChildren<CinemachineBrain>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void FixedUpdate()
    {
        if (isPaused)
            _player.transform.rotation = _rot;
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            if (pausePanel != null)
                pausePanel.SetActive(true);

            _cam.enabled = false;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _rot = _player.transform.rotation;

            isPaused = true;
            StartCoroutine(FreezeRot());
        }
        else
        {
            if (pausePanel != null)
                pausePanel.SetActive(false);

            _cam.enabled = true;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            StopCoroutine(FreezeRot());

            isPaused = false;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isPaused)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    IEnumerator FreezeRot()
    {
        while (true)
        {
            _player.transform.rotation = _rot;

            yield return new WaitForSecondsRealtime(0.001f);
        }
    }
}
