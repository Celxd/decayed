using UnityEngine;
using UnityEngine.UI;

public class Unhide : MonoBehaviour
{
    public GameObject panel;
    public GameObject hidden;

    public void ShowPanel()
    {
        panel.SetActive(true);
        if (hidden.activeSelf)
        {
            hidden.SetActive(false);
        }
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }

}
