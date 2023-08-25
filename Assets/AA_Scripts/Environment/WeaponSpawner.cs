using UnityEngine;

[System.Serializable]
public class PrefabData
{
    public GameObject prefab;
    public int numberOfInstances = 1;
}

public class WeaponSpawner : MonoBehaviour
{
    public PrefabData[] itemPrefabs; 
    public float spawnRadius = 5f;

    private bool hasSpawned = false;

    private void Start()
    {
        if (!hasSpawned)
        {
            SpawnItems();
            hasSpawned = true;
        }
    }

    private void SpawnItems()
    {
        foreach (PrefabData prefabData in itemPrefabs)
        {
            for (int i = 0; i < prefabData.numberOfInstances; i++)
            {
                Vector3 randomSpawnPosition = Random.insideUnitSphere * spawnRadius;
                randomSpawnPosition.y = 0f; 

                GameObject spawnedItem = Instantiate(prefabData.prefab, transform.position + randomSpawnPosition, Quaternion.identity);
       
            }
        }
    }
}
