using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform m_player;
    [SerializeField] GameObject m_enemy;
    [SerializeField] List<Transform> m_spawnPoints;

    [Header("Settings")]
    [SerializeField] float m_spawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
