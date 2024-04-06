using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherChaseState : EnemyChase
{
    public ArcherChaseState(Archer archer,Status status,EnemyStateMachine archerStateMachine) : base(archer,status,archerStateMachine)
    {

    }

    

    public override void Enter()
    {
        base.Enter();
    }

    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
