using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LayerMask m_GroundLayer;
    [SerializeField] LayerMask m_PlayerLayer;
    NavMeshAgent m_Agent;
    Transform m_Player;

    //Stuff
    [Header("Settings")]
    [SerializeField] float m_SightRange;
    [SerializeField] float m_AttackRange;
    [SerializeField] float health;
    bool m_PlayerInSight, m_PlayerInAttack;

    //Patroling
    [Header("Patrolling Settings")]
    [SerializeField] Vector3 m_WalkPoint;
    [SerializeField] bool m_WalkPointSet;
    [SerializeField] float m_WalkPointRange;
    [SerializeField] List<Transform> m_WalkPoints;
    [SerializeField] int m_CurrentWalkPointIndex = 0;

    //Attacking
    float m_AttackInterval;
    bool m_alreadyAttacked;

    private void Awake()
    {
        m_Player = GameObject.Find("Player").transform;
        m_Agent = gameObject.GetComponent<NavMeshAgent>();

        m_CurrentWalkPointIndex = 0;
        m_Agent.SetDestination(m_WalkPoints[m_CurrentWalkPointIndex].position);
        m_Agent.destination = m_WalkPoints[m_CurrentWalkPointIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        m_PlayerInSight = Physics.CheckSphere(transform.position, m_SightRange, m_PlayerLayer);
        m_PlayerInAttack = Physics.CheckSphere(transform.position, m_AttackRange, m_PlayerLayer);

        if (!m_PlayerInSight && !m_PlayerInAttack) Patroling();
        if (m_PlayerInSight && !m_PlayerInAttack) ChasePlayer();
        if (m_PlayerInAttack && m_PlayerInSight) AttackPlayer();

        Debug.Log( m_CurrentWalkPointIndex);
    }

    void Patroling()
    {
        //if (!m_WalkPointSet) SearchWalkPoint();

        //if (m_WalkPointSet)
        m_Agent.SetDestination(m_WalkPoints[m_CurrentWalkPointIndex].position);
        m_Agent.destination = m_WalkPoints[m_CurrentWalkPointIndex].position;

        if (m_Agent.remainingDistance <= 1f)
        {
            m_CurrentWalkPointIndex = m_CurrentWalkPointIndex == m_WalkPoints.Count - 1 ? 0 : m_CurrentWalkPointIndex + 1;
            m_Agent.SetDestination(m_WalkPoints[m_CurrentWalkPointIndex].position);
            m_Agent.destination = m_WalkPoints[m_CurrentWalkPointIndex].position;
        }

    }

    //For random patrolling. Saving it for later just in case.
    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-m_WalkPointRange, m_WalkPointRange);
        float randomX = Random.Range(-m_WalkPointRange, m_WalkPointRange);

        m_WalkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(m_WalkPoint, -transform.up, 2f, m_GroundLayer))
            m_WalkPointSet = true;
    }

    void ChasePlayer()
    {
        m_Agent.SetDestination(m_Player.position);
    }

    void AttackPlayer()
    {
        //This commented code below is to freeze enemy once player is inside attack range, use it on ranged enemies
        //m_Agent.SetDestination(transform.position);

        transform.LookAt(m_Player);

        Debug.Log("Attacking player");
    }

    public void TakeDamage(int damage)
    {
        //Create scriptable object for enemy next time.
        //Copy scriptable object data to this scrpt
        //to prevent it being shared.
        //Current health system is temporary.
        health -= damage;

        if (health < 0)
            Die();
    }

    void Die() => Destroy(gameObject);
}
