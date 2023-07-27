using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    float timer;
    Vector3 head;
    public override void StartState(Enemy enemy)
    {
        enemy.LookDir(enemy.m_Player);
        enemy.m_Agent.SetDestination(enemy.transform.position);
        timer = 5;

        enemy.m_AnimManager.Idle();

        head = enemy.transform.Find("Head").transform.position;
    }

    public override void UpdateState(Enemy enemy)
    {
        enemy.LookDir(enemy.m_Player);
        Vector3 target = enemy.transform.forward;
        
        
        target += Random.insideUnitSphere * enemy.m_Inaccuracy;

        Debug.DrawRay(head, target * enemy.m_AttackRange);
        if (Physics.Raycast(enemy.transform.position, target, out RaycastHit hit , enemy.m_AttackRange))
        {
            if ((enemy.m_PlayerLayer.value & (1 << hit.transform.gameObject.layer)) != 0)
                enemy.m_Player.gameObject.GetComponent<Health>().TakeDamage(2);
        }

        enemy.m_PlayerInAttack = Physics.CheckSphere(enemy.transform.position, enemy.m_AttackRange, enemy.m_PlayerLayer);

        if (!enemy.m_FOV.playerOnSight)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else if (timer <= 0)
                enemy.SwitchState(enemy.m_ChasingState);

        }
        else
            timer = 5;
    }
}
