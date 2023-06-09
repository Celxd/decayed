using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintProximity : MonoBehaviour
{
    public GameObject player;
    public float proximityDistance = 5f;
    public GameObject panel;
    public string sceneName;

    private bool isPlayerInRange = false;

    private void Start()
    {
        panel.SetActive(false);
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
            SceneManager.LoadScene(sceneName);
        }
    }
}
