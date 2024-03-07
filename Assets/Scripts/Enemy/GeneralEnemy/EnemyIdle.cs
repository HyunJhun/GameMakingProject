using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(Enemy enemy,Status stats,EnemyStateMachine enemyStateMachine) : base(enemy,stats,enemyStateMachine)
    { }

    // Local Var
    private float f_timer;

    public override void Enter()
    {
        enemy.b_isIdle = true;
        f_timer = 0f;
    }

    public override void StateActionUpdate()
    {
        if(f_timer < 2f)
        {
            f_timer += Time.deltaTime;
        }
        else
        {
            enemyStateMachine.ChangeState(enemy.patrolState);
            return;
        }
    }

    public override void Exit()
    {
        enemy.b_isIdle = false;
    }
}
