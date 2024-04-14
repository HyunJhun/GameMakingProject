using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected Status stats;
    protected EnemyStateMachine enemyStateMachine;
    public EnemyState(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine)
    {

        this.enemy = enemy;
        this.stats = stats;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void StateActionUpdate() { }

    public virtual void StateActionFixedUpdate() { }
    public virtual void Enter() {}
    public virtual void Exit() { }
}
