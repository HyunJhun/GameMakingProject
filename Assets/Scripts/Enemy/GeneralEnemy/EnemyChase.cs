using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyState
{
    public EnemyChase(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    public override void Enter()
    {
        enemy.GetAnimator().SetBool("isChase", true);
        enemy.SetAgentSpeed(enemy.f_enemyChasingSpeed);
        enemy.GetEnemyNavMeshAgent().stoppingDistance = enemy.f_chaseStopingDistacne;
    }

    public override void StateActionUpdate()
    {
        if(enemy.GetAttackRange().b_isPlayerInRangeOfAttack)
        {
            enemyStateMachine.ChangeState(enemy.readyForAttackState);
            return;
        }

        if(Vector3.Distance(enemy.GetPlayer().transform.position,enemy.transform.position) > enemy.f_chaseMaxDistance)
        {
            enemyStateMachine.ChangeState(enemy.patrolState);
            return;
            
        } 
        enemy.GetEnemyNavMeshAgent().SetDestination(enemy.GetPlayer().transform.position);
        
        
    }
    public override void StateActionFixedUpdate()
    {
        base.StateActionFixedUpdate();
    }

    public override void Exit()
    {
        enemy.GetAnimator().SetBool("isChase", false);
        enemy.GetEnemyNavMeshAgent().SetDestination(enemy.transform.position);
    }
}
