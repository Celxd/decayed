using Andtech.ProTracer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent m_Agent;
    public EnemyRagdoll m_Ragdoll;
    public LayerMask m_GroundLayer;
    public LayerMask m_PlayerLayer;
    public Transform m_Player;
    public EnemyAnimManager m_AnimManager;
    public KillCountManager m_KillCountManager;
    public AudioSource m_audio;

    //Stuff
    [Header("Settings")]
    public float m_AttackRange;
    [SerializeField] public float health;
    public bool m_PlayerInAttack;
    public EnemyFOV m_FOV;
    public float m_Inaccuracy;

    //Patroling
    [Header("Patrolling Settings")]
    [SerializeField] public Vector3 m_WalkPoint;
    [SerializeField] public bool m_WalkPointSet;
    [SerializeField] public float m_WalkPointRange;
    [SerializeField] public List<Transform> m_WalkPoints;
    [SerializeField] public int m_CurrentWalkPointIndex = 0;

    //VFX
    [Header("VFX Settings")]
    [SerializeField] Bullet bulletPrefab = default;
    [SerializeField] SmokeTrail smokeTrailPrefab = default;
    [Range(1, 10)]
    [SerializeField] int tracerSpeed = 3;

    //State managing
    EnemyBaseState m_CurrentState;
    public EnemyPatrollingState m_PatrolingState = new EnemyPatrollingState();
    public EnemyChasingState m_ChasingState = new EnemyChasingState();
    public EnemyAttackingState m_AttackState = new EnemyAttackingState();
    public EnemyDeadState m_DeadState = new EnemyDeadState();

    public float delay = 20.0f;

    private void Awake()
    {
        m_FOV = GetComponent<EnemyFOV>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Ragdoll = GetComponent<EnemyRagdoll>();
        m_AnimManager = GetComponent<EnemyAnimManager>();
        m_Player = GameObject.Find("Player").transform;
        m_KillCountManager = GameObject.Find("KillCountManager").GetComponent<KillCountManager>();

        m_PlayerInAttack = Physics.CheckSphere(transform.position, m_AttackRange, m_PlayerLayer);

        m_CurrentState = m_PatrolingState;

        m_audio.Stop();
        m_audio.loop = true;
        m_audio.enabled = false;
    }

    private void Start()
    {
        m_FOV.StartFOV(m_PlayerLayer);
        
        m_CurrentState.StartState(this);
    }

    private void Update()
    {
        if (m_Player == null)
            return;

        m_CurrentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        m_CurrentState = state;
        m_CurrentState.StartState(this);
    }

    public void LookDir(Transform dir)
    {
        Vector3 target = new Vector3(dir.position.x, transform.position.y, dir.position.z);

        transform.LookAt(target);
    }

    public void TakeDamage(float dmg, Vector3 hit)
    {
        health -= dmg;
        if (m_CurrentState == m_PatrolingState)
            SwitchState(m_ChasingState);

        if(health <= 0)
        {
            Destroy(gameObject, delay);
            DeadBehavior(hit);
        }
    }

    void DeadBehavior(Vector3 hit)
    {
        Vector3 forceDir = transform.position - m_Player.position;
        forceDir.y = 1;

        m_Ragdoll.TriggerRagdoll((1000 * forceDir).normalized, hit);
        m_KillCountManager.IncreaseKillCount();
        SwitchState(m_DeadState);
        Destroy(gameObject, delay);
    }

    public void VFX(Vector3 head, Ray raycast)
    {
        // Instantiate the tracer graphics
        Bullet bullet = Instantiate(bulletPrefab);
        SmokeTrail smokeTrail = Instantiate(smokeTrailPrefab);

        // Setup callbacks
        bullet.Completed += OnCompleted;
        smokeTrail.Completed += OnCompleted;

        // Use different tracer drawing methods depending on the raycast
        if (Physics.Raycast(raycast, out RaycastHit hitInfo, 350))
        {
            // Since start and end point are known, use DrawLine
            bullet.DrawLine(head, hitInfo.point, tracerSpeed * 100);
            smokeTrail.DrawLine(head, hitInfo.point, tracerSpeed * 100);
        }
        else
        {
            // Since we have no end point, use DrawRay
            bullet.DrawRay(raycast.origin, raycast.direction, tracerSpeed * 100, 350);
            smokeTrail.DrawRay(raycast.origin, raycast.direction, tracerSpeed * 100, 25.0F);
        }
    }

    private void OnCompleted(object sender, System.EventArgs e)
    {
        // Handle complete event here
        if (sender is TracerObject tracerObject)
        {
            Destroy(tracerObject.gameObject);
        }
    }
}
