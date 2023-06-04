using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    private RaycastHit hit;
    private Renderer lastHighlightedRenderer;

    private void Update()
    {
        // Create a ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Cast the ray and check for a hit
        if (Physics.Raycast(ray, out hit))
        {
            // Get the renderer of the object that was hit
            Renderer renderer = hit.collider.GetComponent<Renderer>();

            // Check if the hit object has a renderer
            if (renderer != null)
            {
                // Check if this is a new object being highlighted
                if (renderer != lastHighlightedRenderer)
                {
                    // Remove highlight from the previous object
                    RemoveHighlight();

                    // Store the current renderer as the last highlighted renderer
                    lastHighlightedRenderer = renderer;

                    // Apply the highlight effect to the current object
                    ApplyHighlight(renderer);
                }
            }
            else
            {
                // If the hit object doesn't have a renderer, remove the highlight from the previous object
                RemoveHighlight();
            }
        }
        else
        {
            // If no objects were hit, remove the highlight from the previous object
            RemoveHighlight();
        }
    }

    private void ApplyHighlight(Renderer renderer)
    {
        // Store the original materials of the renderer
        Material[] originalMaterials = renderer.materials;

        // Create a new array for the highlighted materials
        Material[] highlightedMaterials = new Material[originalMaterials.Length];

        // Create a highlight material and assign it to each element of the highlighted materials array
        for (int i = 0; i < highlightedMaterials.Length; i++)
        {
            highlightedMaterials[i] = new Material(Shader.Find("Outlined/Silhouetted Diffuse")); // Replace with your desired highlight shader
        }

        // Assign the highlighted materials array to the renderer
        renderer.materials = highlightedMaterials;
    }

    private void RemoveHighlight()
    {
        // Check if there was a previously highlighted renderer
        if (lastHighlightedRenderer != null)
        {
            // Restore the original materials of the renderer
            lastHighlightedRenderer.materials = lastHighlightedRenderer.GetComponent<Renderer>().materials;

            // Reset the last highlighted renderer
            lastHighlightedRenderer = null;
        }
    }
}
