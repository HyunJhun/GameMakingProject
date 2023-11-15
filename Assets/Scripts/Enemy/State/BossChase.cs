using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChase : BossState
{
    public BossChase(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {
        
    }

    // Detect Range
    private float chaseRange;
    private float rushAttackRange;
    private float basicAttackRange;

    // Value
    private int gatcha;
    private float coolTime_BasicAttack;
    public float coolTime_RushAttack;
    public override void Enter()
    {
        // 만약 버그등으로 인해 상태가 잘못 들어왔을시를 방지
        if(!boss.detectPlayer.isDetectPlayer && bossStateMachine.previousState != boss.stiffnessState) 
        {
            bossStateMachine.ChangeState(boss.idleState);
            return;
        }
        //boss.agent.stoppingDistance = rushAttackRange; // 공격 사거리 재조정

        chaseRange = boss.detectPlayer.gameObject.transform.lossyScale.x;
        rushAttackRange = boss.detectPlayer_AttackRange.gameObject.transform.lossyScale.x / 2f + 2f;
        basicAttackRange = boss.detectPlayer_AttackRange.gameObject.transform.lossyScale.x / 2f;
    }
    public override void Exit()
    {
        boss.agent.SetDestination(boss.transform.position); // chase 상태를 벗어나면 기본적으로 추적을 종료하는 개념이기에 멈추어야함
        boss.agent.stoppingDistance = 2f;
    }
    public override void StateActionUpdate()
    {
        if (coolTime_RushAttack <= 3f) coolTime_RushAttack += Time.deltaTime;
        
        // 현재 탐지 범위의 반지름은 7.5 ... distance가 7.5가 최대치여야함
        float distance = Mathf.Abs(Vector3.Distance(boss.gameObject.transform.position, boss.player.transform.position));
        // 보스 회전
        if (distance <= chaseRange) // 추격 범위 설정
        {
            boss.agent.SetDestination(boss.player.transform.position);
            
            if (distance < rushAttackRange && coolTime_RushAttack >= 3f) // 공격 사정 거리
            {
                if (Random.Range(0, 100) <= 45)
                {
                    boss.attackState.patternSelectNumber = 1; // 패턴 번호를 이용해 공격 패턴을 가져감
                    bossStateMachine.ChangeState(boss.attackState);
                    Debug.Log("러쉬");
                }
                coolTime_RushAttack = 0f;
            }
            if(distance < basicAttackRange)
            {
                boss.attackState.patternSelectNumber = 0;
                bossStateMachine.ChangeState(boss.attackState);
                Debug.Log("베이직");
                return;
            }
        }
        else // 추격 범위를 벗어나면
        {
            boss.agent.SetDestination(boss.transform.position); // 추격 범위를 벗어나게 된다면 보스몬스터의 움직임이 멈춰야함. 즉, 위치가 고정되어야 함
            bossStateMachine.ChangeState(boss.stiffnessState);
            return;
        }

    }
}
