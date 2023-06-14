using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void StartState(Enemy enemy);
    public abstract void UpdateState(Enemy enemy);
    public abstract void OnCollisionEnter(Enemy enemy);

}
