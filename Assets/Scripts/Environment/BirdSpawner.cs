using UnityEngine;
using System.Collections;

public class BirdGenerator : MonoBehaviour
{

    public GameObject birdPrefab;
    public int numBirds = 10;
    public float spawnRadius = 10f;
    public float spawnInterval = 5f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            SpawnBird();
            timer = 0f;
        }
    }

    void SpawnBird()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity) as GameObject;
    }
}
