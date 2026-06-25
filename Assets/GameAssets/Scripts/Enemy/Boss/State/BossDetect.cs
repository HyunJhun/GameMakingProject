using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetect : BossState
{
    public BossDetect(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {

    }
    private float timer;
    public override void Enter()
    {
        timer = 0f;
        boss.bossHpBar.SetActive(true);
    }
    public override void Exit()
    {
    }
    public override void StateActionUpdate()
    {
        if(timer < 2.5f) // 보스가 플레이어를 인식하기까지 걸리는 시간
        {
            timer += Time.deltaTime;
            if(!boss.detectPlayer.isDetectPlayer) // 만약 인식 시간 안에 플레이어가 탐지 범위를 벗어나면 
            {
                bossStateMachine.ChangeState(boss.idleState);
                return;
            }
        }
        else
        {
            if (boss.detectPlayer.isDetectPlayer)
            {
                bossStateMachine.ChangeState(boss.chaseState);
                return;
            }
        }
    }
}
