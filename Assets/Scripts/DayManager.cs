using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    public TMP_Text dayText;
    public Health health;

    private int currentDay = 1;

    private void Start()
    {

        StartCoroutine(UpdateDayRoutine());
    }


    private System.Collections.IEnumerator UpdateDayRoutine()
    {
        while (true)
        {
            UpdateDayUI();
            yield return new WaitForSeconds(120f); 
            NextDay();
        }
    }


    public void NextDay()
    {
        currentDay++;
    }


    public void SetDay(int day)
    {
        currentDay = Mathf.Max(1, day); 
    }

    private void UpdateDayUI()
    {
        if (dayText != null)
        {
            dayText.text = "Day " + currentDay.ToString();
        }
    }
}
