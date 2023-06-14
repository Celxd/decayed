using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    public override void StartState(Enemy enemy)
    {
        enemy.m_CurrentWalkPointIndex = 0;
        enemy.m_Agent.SetDestination(enemy.m_WalkPoints[enemy.m_CurrentWalkPointIndex].position);
        enemy.m_Agent.destination = enemy.m_WalkPoints[enemy.m_CurrentWalkPointIndex].position;
        
    }

    public override void UpdateState(Enemy enemy)
    {
        if (enemy.m_Agent.remainingDistance <= 1f)
        {
            enemy.m_CurrentWalkPointIndex = enemy.m_CurrentWalkPointIndex == enemy.m_WalkPoints.Count - 1 ? 0 : enemy.m_CurrentWalkPointIndex + 1;
            enemy.m_Agent.SetDestination(enemy.m_WalkPoints[enemy.m_CurrentWalkPointIndex].position);
            enemy.m_Agent.destination = enemy.m_WalkPoints[enemy.m_CurrentWalkPointIndex].position;
        }

        enemy.m_PlayerInSight = Physics.CheckSphere(enemy.transform.position, enemy.m_SightRange, enemy.m_PlayerLayer);
        if (enemy.m_PlayerInSight)
            enemy.SwitchState(enemy.m_ChasingState);
    }

    public override void OnCollisionEnter(Enemy enemy) { }
}
