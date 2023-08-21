using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public string[] levelNames;

    private int currentLevelIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levelNames.Length)
        {
            SceneManager.LoadScene(levelNames[currentLevelIndex]);
            currentLevelIndex++;
        }
    }

    //public void CompleteCurrentLevel()
    //{
    //    if (currentLevelIndex > 0)
    //    {
    //        LevelUnlockSystem.instance.UnlockNextLevel();
    //    }
    //}
}
