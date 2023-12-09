using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDead : BossState
{
    // Start is called before the first frame update
    public BossDead(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine) { }

    public override void Enter()
    {
        boss.isDie = true;
        boss.bossAnimationHandler.GetBossAnimator().SetBool("isDie", boss.isDie);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
    }
}
