using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossBack : BossState
{

    public BossBack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine) { }

    public override void Enter()
    {
        Debug.Log("BackEnter");
    }

    public override void Exit()
    {
        Debug.Log("BackExit");
    }

    public override void StateActionUpdate()
    {
        Vector3 bossPos = boss.gameObject.transform.position;
        Vector3 backDirection = bossPos - boss.backPoint.transform.position;

        if (boss.isBack) // trigger로 체크
        {
            Debug.Log("도착");
            boss.isBack = false; // 나중에 다시 돌아오는 걸 위해
            bossStateMachine.ChangeState(boss.idleState);
            return;
        }
        else
        {
            boss.agent.destination = boss.backPoint.transform.position;  
        }
    }
}
