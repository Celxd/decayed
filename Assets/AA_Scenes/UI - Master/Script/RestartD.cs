using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartD : MonoBehaviour
{
    public void restart()
    {
        SceneManager.LoadScene("Decayed");
    }
}
