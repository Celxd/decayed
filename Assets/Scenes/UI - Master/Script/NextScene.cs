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
    public void Apartment()
    {
        SceneManager.LoadScene("Apartment");
    }
    public void Load()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }
    public void NC()
    {
        SceneManager.LoadScene("NC");
    }
    public void Audio()
    {
        SceneManager.LoadScene("Audio");
    }
    public void Display()
    {
        SceneManager.LoadScene("Display");
    }

    // This function quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
