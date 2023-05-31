using UnityEngine;
using System.Collections;

public class CowSpawner : MonoBehaviour
{

    public GameObject cowprefab;
    public int numcow = 10;
    public float spawnRadius = 10f;
    public float spawnInterval = 5f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnInterval)
        {
            Spawncow();
            timer = 0f;
        }
    }

    void Spawncow()
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        GameObject cow = Instantiate(cowprefab, spawnPosition, Quaternion.identity) as GameObject;
    }
}
