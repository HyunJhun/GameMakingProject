using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlightAttack : BossState
{
/*
1. 파이어볼 발사
2. 날아가면서 불똥 투하(밝거나 시간 지나면 터짐)
3. 가끔 날아가다가 하강해서 박치기
4. 잠시 하강해서 휴식(딜타임)
 */
    public BossFlightAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {

    }
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
    }

    public override void StateActionFixedUpdate()
    {
        base.StateActionFixedUpdate();
    }
}
