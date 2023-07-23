using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    float timer;
    public override void StartState(Enemy enemy)
    {
        enemy.transform.LookAt(enemy.m_Player);
        enemy.m_Agent.SetDestination(enemy.transform.position);
        timer = 10;
    }

    public override void UpdateState(Enemy enemy)
    {
        enemy.transform.LookAt(enemy.m_Player);
        if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, enemy.m_AttackRange, enemy.m_PlayerLayer))
        {
            //enemy.m_Player.gameObject.GetComponent<Health>().TakeDamage(50 * Time.deltaTime);
            Debug.Log(enemy.m_Player.gameObject.GetComponent<Health>().health);
        }

        enemy.m_PlayerInAttack = Physics.CheckSphere(enemy.transform.position, enemy.m_AttackRange, enemy.m_PlayerLayer);

        if (!enemy.m_FOV.playerOnSight)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0)
            {
                enemy.SwitchState(enemy.m_ChasingState);
            }

        }
        else
            timer = 10;
    }

    public override void OnCollisionEnter(Enemy enemy)
    {

    }
}
