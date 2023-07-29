using UnityEngine;

public class PanelMover : MonoBehaviour
{
    public Transform designatedPosition;
    private RectTransform panelRectTransform;
    private Vector3 originalPosition;

    private void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
        originalPosition = panelRectTransform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (panelRectTransform.position == originalPosition)
            {
                MovePanelToDesignatedPosition();
            }
            else
            {
                MovePanelToOriginalPosition();
            }
        }
    }

    private void MovePanelToDesignatedPosition()
    {
        if (designatedPosition == null || panelRectTransform == null)
        {
            Debug.LogWarning("Designated position or panel RectTransform not set!");
            return;
        }

        panelRectTransform.position = designatedPosition.position;
    }

    private void MovePanelToOriginalPosition()
    {
        panelRectTransform.position = originalPosition;
    }
}
