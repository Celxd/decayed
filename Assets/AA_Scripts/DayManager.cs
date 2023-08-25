using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    public TMP_Text[] dayTexts;

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
        foreach (var dayText in dayTexts)
        {
            if (dayText != null)
            {
                dayText.text = "Day " + currentDay.ToString();
            }
        }
    }
}
