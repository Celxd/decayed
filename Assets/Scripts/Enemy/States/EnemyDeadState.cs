using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    // Start is called before the first frame update
    public override void StartState(Enemy enemy)
    {
        Debug.Log("dead");
        enemy.m_Agent.SetDestination(enemy.transform.position);
    }

    // Update is called once per frame
    public override void UpdateState(Enemy enemy)
    {
        
    }
}
