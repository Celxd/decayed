using UnityEngine;
using UnityEngine.UI;

public class CreditsRoll : MonoBehaviour
{
    public float scrollSpeed = 50.0f;

    void Update()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 position = rt.anchoredPosition;
        position.y += scrollSpeed * Time.deltaTime;
        rt.anchoredPosition = position;
    }
}
