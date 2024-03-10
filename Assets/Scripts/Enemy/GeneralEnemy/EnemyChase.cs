using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyState
{
    public EnemyChase(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    public override void Enter()
    {
        enemy.b_isChase = true;
        enemy.SetAgentSpeed(enemy.f_enemyChasingSpeed);
        enemy.GetEnemyNavMeshAgent().stoppingDistance = enemy.f_chaseStopingDistacne;
    }

    public override void StateActionUpdate()
    {
        if(Vector3.Distance(enemy.GetPlayer().transform.position,enemy.transform.position) < 20f)
        {
            enemy.GetEnemyNavMeshAgent().SetDestination(enemy.GetPlayer().transform.position);
        }
        else
        {
            enemyStateMachine.ChangeState(enemy.returnState);
            return;
        }
        if(enemy.GetAttackRange().b_isPlayerInRangeOfAttack)
        {
            enemyStateMachine.ChangeState(enemy.attackState);
            return;
        }
    }
    public override void StateActionFixedUpdate()
    {
        base.StateActionFixedUpdate();
    }

    public override void Exit()
    {
        enemy.b_isChase = false;
    }
}
