using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    public override void StartState(Enemy enemy)
    {
        enemy.m_Agent.SetDestination(enemy.m_Player.position);
    }

    public override void UpdateState(Enemy enemy)
    {
        enemy.m_Agent.SetDestination(enemy.m_Player.position);

        enemy.m_PlayerInAttack = Physics.CheckSphere(enemy.transform.position, enemy.m_AttackRange, enemy.m_PlayerLayer);
        enemy.m_PlayerInSight = Physics.CheckSphere(enemy.transform.position, enemy.m_SightRange, enemy.m_PlayerLayer);
        if (enemy.m_PlayerInAttack)
            enemy.SwitchState(enemy.m_AttackState);

        if(!enemy.m_PlayerInSight)
            enemy.SwitchState(enemy.m_PatrolingState);
    }

    public override void OnCollisionEnter(Enemy enemy) { }
}
