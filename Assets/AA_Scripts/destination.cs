using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destination : MonoBehaviour
{
    public GameObject panel;
    public GameObject cutsscenepanel;
    public GameObject Player;
    public Transform destinationPortal;
    private bool isTeleporting = false;

    void Start()
    {
        Button teleportButton = GetComponent<Button>();
        teleportButton.onClick.AddListener(TeleportPlayer);
    }

    private void TeleportPlayer()
    {
        if (!isTeleporting && Player != null)
        {
            isTeleporting = true;

            Vector3 destinationPosition = destinationPortal.position;


            Player.transform.position = destinationPosition;

            isTeleporting = false;
            panel.SetActive(false);
            LockCursor();
            Cutscenes();
        }
    }
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Cutscenes()
    {
        cutsscenepanel.SetActive(true);  
    }
    private void Off()
    {
        cutsscenepanel.SetActive(false);
    }

}
