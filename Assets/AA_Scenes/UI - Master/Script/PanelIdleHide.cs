using UnityEngine;

public class PanelIdleHide : MonoBehaviour
{
    public float idleTime = 5f; 
    private float lastInputTime;

    private void OnEnable()
    {
        lastInputTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - lastInputTime >= idleTime)
        {
            HidePanel();
        }

        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            lastInputTime = Time.time;
            ShowPanel();
        }
    }

    private void HidePanel()
    {
        gameObject.SetActive(false);
    }

    private void ShowPanel()
    {
        gameObject.SetActive(true);
        lastInputTime = Time.time;
    }
}
