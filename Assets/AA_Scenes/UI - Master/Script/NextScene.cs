using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void Menu()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Menu")
        {
            
            if (!SceneManager.GetSceneByName("Menu").isLoaded)
            {
                SceneManager.LoadScene(currentSceneName);
            }
        }
        else
        {
            
            SceneManager.LoadScene("Menu");
        }
    }


    //public void Haykal()
    //{
    //    SceneManager.LoadScene("Haykal");
    //}
    public void Apartment()
    {
        SceneManager.LoadScene("Apartment");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NC()
    {
        SceneManager.LoadScene("NC");
    }
   
    public void QuitGame()
    {
        Application.Quit();
    }
}
