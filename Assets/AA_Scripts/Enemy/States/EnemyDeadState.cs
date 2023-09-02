using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    float timer = 3f;

    // Start is called before the first frame update
    public override void StartState(Enemy enemy)
    {
        enemy.m_Agent.SetDestination(enemy.transform.position);
        enemy.m_audio.enabled = false;
        
    }

    // Update is called once per frame
    public override void UpdateState(Enemy enemy)
    {
        //if (timer <= 0)
        //{
        //    enemy.gameObject.Destroy(enemy.gameObject);
        //}
        //else
        //    timer -= Time.deltaTime;
    }
}
