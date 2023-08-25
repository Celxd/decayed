using UnityEngine;

public class ObjectClickSound : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    private void OnMouseDown()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
