using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingPanel;
    public Slider loadingBar;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadSceneAsync(levelName));
    }

    IEnumerator LoadSceneAsync(string levelName)
    {
        loadingPanel.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            loadingBar.value = progress;

            yield return null;
        }
    }
}
