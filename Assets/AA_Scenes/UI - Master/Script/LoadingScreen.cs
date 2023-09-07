using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingBar;
    public TMP_Text loadingText; 

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadSceneAsync(levelName));
    }

    private IEnumerator LoadSceneAsync(string levelName)
    {
        loadingPanel.SetActive(true); 

        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            loadingBar.value = progress * 100f; 

     
            loadingText.text = $"Loading: {Mathf.Round(progress * 100f)}%";

            yield return null; 
        }

        loadingPanel.SetActive(false);
        loadingText.text = "Loading Complete";
    }
}
