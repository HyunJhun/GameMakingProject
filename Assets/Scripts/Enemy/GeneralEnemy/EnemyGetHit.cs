using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : EnemyState
{
    public EnemyGetHit(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    public override void Enter()
    {
        enemy.GetAnimator().SetTrigger("GetHit");
        enemy.GetEnemyNavMeshAgent().enabled = false;
    }

    public override void StateActionUpdate()
    {
        if(enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
        {
            enemyStateMachine.ChangeState(enemy.patrolState);
            return;
        }
    }

    public override void StateActionFixedUpdate()
    {
        base.StateActionFixedUpdate();
    }

    public override void Exit()
    {
        enemy.GetEnemyNavMeshAgent().enabled = true;
        enemy.b_isGetHit = false;
    }
}
