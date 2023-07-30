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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            yield return new WaitForSeconds(m_spawnInterval);

            Transform spawnPoint = m_spawnPoints.Count > 1 ? m_spawnPoints[Random.Range(0, m_spawnPoints.Count)] : m_spawnPoints[0];

            //Check is player not too far
            Collider[] player = Physics.OverlapSphere(spawnPoint.transform.position, m_checkRadius, m_playerMask);
            if (player.Length == 0)
                yield break;

            //Check is player not too close
            if (Vector3.Distance(spawnPoint.transform.position, player[0].transform.position) >= m_checkRadius / 2)
            {
                GameObject enemyObject = Instantiate(m_enemy, spawnPoint.transform.position, spawnPoint.rotation, m_enemyParent);
                Enemy enemy = enemyObject.GetComponent<Enemy>();

                enemy.SwitchState(enemy.m_ChasingState);
            }
        }
        
    }
}
