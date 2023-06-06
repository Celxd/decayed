using UnityEngine;
using UnityEngine.UI;

public class DissapearingText : MonoBehaviour
{
    public float disappearDelay = 3f; // Number of seconds before the text disappears
    private Text textComponent;
    private bool isCounting = false;

    private void Start()
    {
        textComponent = GetComponent<Text>();
    }

    private void Update()
    {
        if (isCounting)
        {
            disappearDelay -= Time.deltaTime;
            if (disappearDelay <= 0)
            {
                Destroy(textComponent.gameObject);
                isCounting = false;
            }
        }
    }

    public void StartCountdown()
    {
        textComponent.enabled = true;
        isCounting = true;
        disappearDelay = 3f; // Reset the countdown
    }
}