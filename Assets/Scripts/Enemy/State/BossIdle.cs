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
        Debug.Log("IdleEnter");
    }
    public override void Exit()
    {
        Debug.Log("IdleExit");
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
