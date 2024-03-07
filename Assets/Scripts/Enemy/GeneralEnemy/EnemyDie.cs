using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : EnemyState
{
    public EnemyDie(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
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
