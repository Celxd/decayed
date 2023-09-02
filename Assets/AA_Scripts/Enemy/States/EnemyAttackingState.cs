using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    float timer;
    Vector3 head;
    float diTimer = 0f;
    float cdShoot;
    public override void StartState(Enemy enemy)
    {
        enemy.LookDir(enemy.m_Player);
        enemy.m_Agent.SetDestination(enemy.transform.position);
        timer = 5;

        enemy.m_AnimManager.Idle();

        head = enemy.transform.Find("Head").transform.position;

        cdShoot = 1 / 600 / 60;

        enemy.m_audio.enabled = true;
    }

    public override void UpdateState(Enemy enemy)
    {
        enemy.LookDir(enemy.m_Player);
        Vector3 target = enemy.transform.forward;
        
        
        target += Random.insideUnitSphere * enemy.m_Inaccuracy;

        if(cdShoot == 0)
        {
            enemy.VFX(head);

            //enemy.m_audio.PlayOneShot(enemy.m_audio.clip, 1f);
            if (Physics.Raycast(head, target, out RaycastHit hit, enemy.m_AttackRange))
            {
                if ((enemy.m_PlayerLayer.value & (1 << hit.transform.gameObject.layer)) != 0)
                {
                    enemy.m_Player.gameObject.GetComponent<Health>().TakeDamage(2);
                    if (diTimer > 0)
                    {
                        diTimer -= 1;
                    }
                    else
                    {
                        DI_System.CreateIndicator(enemy.transform);
                        diTimer = 3f;
                    }

                }
            }
            cdShoot = 1 / 600 / 60;
        } 
        else
        {
            cdShoot -= Time.deltaTime;
        }
        

        enemy.m_PlayerInAttack = Physics.CheckSphere(enemy.transform.position, enemy.m_AttackRange, enemy.m_PlayerLayer);

        if (!enemy.m_FOV.playerOnSight)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else if (timer <= 0)
            {
                enemy.SwitchState(enemy.m_ChasingState);

                enemy.m_audio.enabled = false;
            }
                

        }
        else
            timer = 5;
    }
}
