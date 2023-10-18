using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossState
{
    public BossAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("DetectEnter");
    }
    public override void Exit()
    {
        Debug.Log("DetectExit");
    }
    public override void StateActionUpdate()
    {
        Debug.Log("²ô¾Æ¾Æ");
    }
}
