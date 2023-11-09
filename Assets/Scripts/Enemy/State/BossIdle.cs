using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : BossState
{
    public BossIdle(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {

    }

    public override void Enter() 
    {
    }
    public override void Exit()
    {
    }
    public override void StateActionUpdate()
    {
        if (boss.detectPlayer.isDetectPlayer)
        {
            bossStateMachine.ChangeState(boss.detectState);
            return;
        }
    }
}
