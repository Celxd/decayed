using UnityEngine;

public class Initiatedialogue : MonoBehaviour
{
    public string[] dialogue;

    private void Start()
    {
        // TODO: Initialize NPC's dialogue here
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Display NPC's dialogue here
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Hide NPC's dialogue here
        }
    }
}
