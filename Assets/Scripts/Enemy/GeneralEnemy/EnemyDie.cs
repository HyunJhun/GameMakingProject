using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : EnemyState
{
    public EnemyDie(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }
    public override void Enter()
    {
        enemy.GetEnemyNavMeshAgent().enabled = false;
        enemy.enabled = false;
        enemy.GetAnimator().SetTrigger("Die");
        GameObject.Destroy(enemy.gameObject,5f);
    }

    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
