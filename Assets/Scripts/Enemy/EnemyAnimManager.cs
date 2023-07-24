using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimManager : MonoBehaviour
{
    Animator m_Animator;
    NavMeshAgent m_Agent;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.enabled = true;

        m_Agent = GetComponent<NavMeshAgent>();
    }

    public void Idle()
    {
        m_Animator.SetBool("Walk", false);
        m_Animator.SetBool("Sprint", false);

        m_Agent.speed = 1;
    }

    public void Walk()
    {
        m_Animator.SetBool("Walk", true);
        m_Animator.SetBool("Sprint", false);

        m_Agent.speed = 1;

    }

    public void Sprint()
    {
        m_Animator.SetBool("Walk", false);
        m_Animator.SetBool("Sprint", true);

        m_Agent.speed = 4;
    }
}
