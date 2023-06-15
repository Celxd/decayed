using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public LayerMask m_GroundLayer;
    [SerializeField] public LayerMask m_PlayerLayer;
    public NavMeshAgent m_Agent;
    public Transform m_Player;

    //Stuff
    [Header("Settings")]
    public float m_SightRange;
    public float m_AttackRange;
    [SerializeField] float health;
    public bool m_PlayerInSight, m_PlayerInAttack;

    //Patroling
    [Header("Patrolling Settings")]
    [SerializeField] public Vector3 m_WalkPoint;
    [SerializeField] public bool m_WalkPointSet;
    [SerializeField] public float m_WalkPointRange;
    [SerializeField] public List<Transform> m_WalkPoints;
    [SerializeField] public int m_CurrentWalkPointIndex = 0;

    //State managing
    EnemyBaseState m_CurrentState;
    public EnemyPatrollingState m_PatrolingState = new EnemyPatrollingState();
    public EnemyChasingState m_ChasingState = new EnemyChasingState();
    public EnemyAttackingState m_AttackState = new EnemyAttackingState();

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.Find("Player").transform;

        m_PlayerInSight = Physics.CheckSphere(transform.position, m_SightRange, m_PlayerLayer);
        m_PlayerInAttack = Physics.CheckSphere(transform.position, m_AttackRange, m_PlayerLayer);

        m_CurrentState = m_PatrolingState;

        m_CurrentState.StartState(this);
    }

    private void Update()
    {
        m_CurrentState.UpdateState(this);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }

    public void SwitchState(EnemyBaseState state)
    {
        m_CurrentState = state;
        m_CurrentState.StartState(this);
    }
}
