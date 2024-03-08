using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : EnemyState
{
    public EnemyDetect(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    // Local var
    float timer;
    public override void Enter()
    {
        timer = 0f;
        enemy.GetEnemyNavMeshAgent().SetDestination(enemy.transform.position);
    }

    public override void StateActionUpdate()
    {
        if(timer < 2.5f)
        {
            timer += Time.deltaTime;
            if(!enemy.GetDetectPlayerRange().isDetectPlayer)
            {
                enemyStateMachine.ChangeState(enemy.patrolState);
                return;
            }
        }
        else
        {
            enemyStateMachine.ChangeState(enemy.chaseState);
            return;
        }
    }
    public override void StateActionFixedUpdate()
    {
        base.StateActionFixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
