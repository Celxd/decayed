using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // This function loads a new scene based on its name
    public void LoadScene()
    {
        SceneManager.LoadScene("Menu");
    }

    // This function quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
