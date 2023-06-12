using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    public float highlightThickness = 0.1f;
    public Color highlightColor = Color.yellow;

    private Material originalMaterial;
    private Material highlightMaterial;
    private Renderer itemRenderer;

    private void Start()
    {
        
        itemRenderer = GetComponent<Renderer>();
        originalMaterial = itemRenderer.material;

       
        highlightMaterial = new Material(originalMaterial);
        highlightMaterial.SetFloat("_Outline", highlightThickness);
        highlightMaterial.SetColor("_OutlineColor", highlightColor);
    }

    private void OnMouseOver()
    {
      
        itemRenderer.material = highlightMaterial;
    }

    private void OnMouseExit()
    {
        
        itemRenderer.material = originalMaterial;
    }
}
