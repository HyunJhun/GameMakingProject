using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReadyForAttack : EnemyState
{
    public EnemyReadyForAttack(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    protected float currentMovingTime;
    protected float direction;
    public override void Enter()
    {
        currentMovingTime = 0f;
        direction = selectMoveDirection();
        //enemy.GetEnemyNavMeshAgent().enabled = false;
    }

    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
    }
    public override void Exit()
    {
        base.Exit();
    }
    private float selectMoveDirection()
    {
        int randomNumber = Random.Range(1, 3);
        return (randomNumber == 1) ? -1f : 1f;
    }
}
