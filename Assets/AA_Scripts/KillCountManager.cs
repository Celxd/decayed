using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillCountManager : MonoBehaviour
{
    public int killCount = 0; 
    public TMP_Text killCountText;

    public void IncreaseKillCount()
    {
        killCount++;
        UpdateKillCountText();
    }

    private void UpdateKillCountText()
    {
        if (killCountText != null)
        {
            killCountText.text = killCount.ToString();
        }
    }
}
