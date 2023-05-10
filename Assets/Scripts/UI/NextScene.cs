using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // This function loads a new scene based on its name
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Haykal()
    {
        SceneManager.LoadScene("Haykal");
    }
    public void Load()
    {
        SceneManager.LoadScene("Loading Screen");
    }
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }

    // This function quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
