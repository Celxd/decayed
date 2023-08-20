using Cinemachine;
using System.Collections;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject Inventorypanel;

    GameObject _player;
    bool isPaused = false;
    CursorLockMode previousLockState;
    bool previousCursorVisibility;
    CinemachineBrain _cam;
    Quaternion _rot;

    private void Awake()
    {
        _player = GameObject.Find("Player");
        _cam = GameObject.Find("Player").GetComponentInChildren<CinemachineBrain>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TogglePause();
        }
    }

    private void FixedUpdate()
    {
        if (isPaused)
        {
            _player.transform.rotation = _rot;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Inventorypanel.SetActive(isPaused);

        if (isPaused) //Pausing
        {
            SaveCursorSettings();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cam.enabled = false;
            StartCoroutine(FreezeRot());
        }
        else //Resume
        {
            Inventorypanel.SetActive(false);
            Time.timeScale = 1;
            RestoreCursorSettings();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cam.enabled = true;
            StopCoroutine(FreezeRot());
        }
        Time.timeScale = isPaused ? 0 : 1;
    }

    IEnumerator FreezeRot()
    {
        while(true)
        {
            _player.transform.rotation = _rot;

            yield return new WaitForSecondsRealtime(0.001f);
        }
    }

    private void SaveCursorSettings()
    {
        previousLockState = Cursor.lockState;
        previousCursorVisibility = Cursor.visible;
        _rot = _player.transform.rotation;
    }

    private void RestoreCursorSettings()
    {
        Cursor.lockState = previousLockState;
        Cursor.visible = previousCursorVisibility;
    }
}
