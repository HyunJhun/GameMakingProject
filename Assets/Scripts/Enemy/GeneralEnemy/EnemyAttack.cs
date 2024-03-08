using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    // Start is called before the first frame update
    public EnemyAttack(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
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

    public void ToDamage(int numOfAttack)
    {

    }
}
