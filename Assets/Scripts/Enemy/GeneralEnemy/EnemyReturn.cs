using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReturn : EnemyState
{
    public EnemyReturn(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    public override void Enter()
    {
        ReturnToInitPoint();
    }

    public override void StateActionUpdate()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.GetEnemyNavMeshAgent().destination) < 1f)
        {
            enemyStateMachine.ChangeState(enemy.idleState);
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

    private void ReturnToInitPoint()
    {
        enemy.GetEnemyNavMeshAgent().SetDestination(enemy.GetInitPosition());
    }
}
