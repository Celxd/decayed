using UnityEngine;
using System.Collections.Generic;

public class BirdGenerator : MonoBehaviour
{
    public GameObject birdPrefab;
    public int numBirds = 10;
    public float spawnRadius = 10f;
    public float spawnInterval = 5f;
    public int birdLimit;

    private float timer = 0f;
    private List<GameObject> spawnedBirds = new List<GameObject>();

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval && spawnedBirds.Count < birdLimit)
        {
            SpawnBird();
            timer = 0f;
        }
    }

    void SpawnBird()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
        spawnedBirds.Add(bird);

 
        if (spawnedBirds.Count > birdLimit)
        {
            GameObject oldestBird = spawnedBirds[0];
            spawnedBirds.RemoveAt(0);
            Destroy(oldestBird);
        }
    }
}
