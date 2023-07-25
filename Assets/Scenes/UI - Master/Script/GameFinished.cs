using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinished : MonoBehaviour
{
    public float panelDelay = 2f;
    public GameObject finishPanel; 

    private bool gameFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gameFinished)
        {
            gameFinished = true;
            ShowPanelAndSwitchScene();
        }
    }

    private void ShowPanelAndSwitchScene()
    {
     
        finishPanel.SetActive(true);

  
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Invoke("SwitchToMenuScene", panelDelay);
    }

    private void SwitchToMenuScene()
    {
        SceneManager.LoadScene("Apartment");
    }
}
