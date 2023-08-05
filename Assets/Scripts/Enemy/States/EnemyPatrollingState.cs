using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
    public override void StartState(Enemy enemy)
    {
        if (enemy.m_WalkPoints == null)
            enemy.SwitchState(enemy.m_ChasingState);

        enemy.m_CurrentWalkPointIndex = 0;
        enemy.m_Agent.SetDestination(enemy.m_WalkPoints[enemy.m_CurrentWalkPointIndex].position);
        enemy.m_Agent.destination = enemy.m_WalkPoints[enemy.m_CurrentWalkPointIndex].position;

        enemy.m_AnimManager.Walk();
    }

    public override void UpdateState(Enemy enemy)
    {
        
        if (enemy.m_Agent.remainingDistance <= 1f)
        {
            enemy.m_CurrentWalkPointIndex = enemy.m_CurrentWalkPointIndex == enemy.m_WalkPoints.Count - 1 ? 0 : enemy.m_CurrentWalkPointIndex + 1;
            enemy.m_Agent.SetDestination(enemy.m_WalkPoints[enemy.m_CurrentWalkPointIndex].position);
            enemy.m_Agent.destination = enemy.m_WalkPoints[enemy.m_CurrentWalkPointIndex].position;
        }

        if (enemy.m_FOV.playerOnSight)
            enemy.SwitchState(enemy.m_ChasingState);
    }
}
