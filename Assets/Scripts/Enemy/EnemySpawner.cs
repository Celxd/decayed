using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform m_player;
    [SerializeField] GameObject m_enemy;
    [SerializeField] Transform m_enemyParent;
    [SerializeField] LayerMask m_playerMask;
    [SerializeField] List<Transform> m_spawnPoints;

    [Header("Settings")]
    [SerializeField] float m_spawnInterval;
    [SerializeField] public float m_checkRadius;
    [SerializeField] float m_countLimit;

    float m_enemyCount;
    bool m_doneSpawning;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = m_spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        m_enemyCount = m_enemyParent.transform.childCount;
        if (timer <= 0)
        {
            if (CanSpawn() && !m_doneSpawning)
            {
                m_doneSpawning = true;
                Spawn();
                timer = m_spawnInterval;

            }
            else if (CanSpawn() && m_doneSpawning)
            {
                m_doneSpawning = false;
                timer = m_spawnInterval;
            }
        }
        else
        {
            Debug.Log(timer);
            timer -= Time.deltaTime;
        }
    }

    bool CanSpawn()
    {
        if (m_enemyCount >= m_countLimit)
            return false;
        else if (m_enemyCount <= m_countLimit - 1)
            return true;
        else
            return false;
    }

    void Spawn()
    {
        Debug.Log("coyrytinse");

        Transform spawnPoint = m_spawnPoints.Count > 1 ? m_spawnPoints[Random.Range(0, m_spawnPoints.Count)] : m_spawnPoints[0];

        //Check is player not too far
        Collider[] player = Physics.OverlapSphere(spawnPoint.transform.position, m_checkRadius, m_playerMask);
        if (player.Length == 0)
            return;

        //Check is player not too close
        if (Vector3.Distance(spawnPoint.transform.position, player[0].transform.position) >= m_checkRadius / 2)
        {
            GameObject enemyObject = Instantiate(m_enemy, spawnPoint.transform.position, spawnPoint.rotation, m_enemyParent);
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            enemy.SwitchState(enemy.m_ChasingState);
        }
    }
}
