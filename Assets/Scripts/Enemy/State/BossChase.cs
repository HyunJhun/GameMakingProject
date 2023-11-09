using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChase : BossState
{
    public BossChase(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {
        
    }


    // Detect Range Radius
    private static float radius = 7.5f;
    private float chaseRange = radius + 5f;
    private float attackRange = radius - 3f;
    

    private float timer;
    public override void Enter()
    {
        // 만약 버그등으로 인해 상태가 잘못 들어왔을시를 방지
        if(!boss.detectPlayer.isDetectPlayer && bossStateMachine.previousState != boss.stiffnessState) 
        {
            bossStateMachine.ChangeState(boss.idleState);
            return;
        }
        timer = 0f;
        boss.agent.stoppingDistance = attackRange;
    }
    public override void Exit()
    {
        boss.agent.destination = boss.transform.position; // chase 상태를 벗어나면 기본적으로 추적을 종료하는 개념이기에 멈추어야함
        boss.agent.stoppingDistance = 0f;
    }
    public override void StateActionUpdate()
    {
        // 현재 탐지 범위의 반지름은 7.5 ... distance가 7.5가 최대치여야함
        float distance = Mathf.Abs(Vector3.Distance(boss.gameObject.transform.position, boss.player.transform.position));
        // 보스 회전
        if (distance <= chaseRange) // 추격 범위 설정
        {
            Debug.Log("추격 범위 안");
           // boss.agent.destination = boss.player.transform.position;
            boss.agent.SetDestination(boss.player.transform.position);
            timer = 0f;
            if (distance < radius - 3f) // 공격 사정 거리
            {
                bossStateMachine.ChangeState(boss.attackState);
                return;
            }
        }
        else // 추격 범위를 벗어나면
        {
            boss.agent.destination = boss.transform.position;
            bossStateMachine.ChangeState(boss.stiffnessState);
            return;
        }

        
    }
}
