using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    private float breathCoolTime = 60f;
    private float rushCoolTime = 10f;
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

        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.Walk, true, boss);
    }
    public override void Exit()
    {
        //boss.agent.SetDestination(boss.transform.position); // chase 상태를 벗어나면 기본적으로 추적을 종료하는 개념이기에 멈추어야함
        boss.agent.stoppingDistance = 2f;
        SoundManager.soundManagerInstacne.StopSfx(boss);
    }
    public override void StateActionUpdate()
    {
        if (checkBossHpForFlightPhase()) // 만약 hp가 50% 미만이라면 페이즈 2로 넘어갈 수 있는지 확인한다
        {
            Debug.Log("피 50 미만");
            if (!boss.isEnterPhaseTwo) bossStateMachine.ChangeState(boss.flightState); // 만약 페이즈가 바뀐적이 없다면 페이즈를 변경한다.
        }
        else
        {
            if (boss.coolTime_RushAttack <= rushCoolTime) boss.coolTime_RushAttack += Time.deltaTime;
            if (boss.coolTime_BreathAttack <= breathCoolTime) boss.coolTime_BreathAttack += Time.deltaTime;
            checkDistanceBetweenBossToPlayer();
        }
        
        
    }
    private void checkDistanceBetweenBossToPlayer()
    {
        float distance = Mathf.Abs(Vector3.Distance(boss.gameObject.transform.position, boss.player.transform.position));

        if (distance <= chaseRange) // 추격 범위 설정
        {
            boss.agent.SetDestination(boss.player.transform.position);

            bossAttackPatternSelectByDistance(distance);
        }
        else // 추격 범위를 벗어나면
        {
            //boss.agent.SetDestination(boss.transform.position); // 추격 범위를 벗어나게 된다면 보스몬스터의 움직임이 멈춰야함. 즉, 위치가 고정되어야 함
            bossStateMachine.ChangeState(boss.stiffnessState);
            return;
        }
    }
    private void bossAttackPatternSelectByDistance(float distance)
    {
        if(distance < rushAttackRange + 2f && boss.coolTime_BreathAttack >= breathCoolTime)
        {
            //if (Random.Range(0, 100) <= 70)
            //{
            //    boss.attackState.patternSelectNumber = 2;
            //    bossStateMachine.ChangeState(boss.attackState);
            //    Debug.Log("Boss Attack : Breath");
            //    return;
            //}
            boss.attackState.patternSelectNumber = 2;
            bossStateMachine.ChangeState(boss.attackState);
            Debug.Log("Boss Attack : Breath");
            boss.coolTime_BreathAttack = 0f;
            return;
        }
        if (distance < rushAttackRange && boss.coolTime_RushAttack >= rushCoolTime) // 공격 사정 거리
        {
            //if (Random.Range(0, 100) <= 60)
            //{
            //    boss.attackState.patternSelectNumber = 1; // 패턴 번호를 이용해 공격 패턴을 가져감
            //    bossStateMachine.ChangeState(boss.attackState);
            //    Debug.Log("Boss Attack : Rush");
            //    return;
            //}
            boss.attackState.patternSelectNumber = 1; // 패턴 번호를 이용해 공격 패턴을 가져감
            bossStateMachine.ChangeState(boss.attackState);
            Debug.Log("Boss Attack : Rush");
            boss.coolTime_RushAttack = 0f;
            return;
        }
        if (distance < basicAttackRange)
        {
            boss.attackState.patternSelectNumber = 0;
            bossStateMachine.ChangeState(boss.attackState);
            Debug.Log("Boss Attack : Basic");
            return;
        }
    }
    private bool checkBossHpForFlightPhase()
    {
        float bossCurrentHp = stats.getHp();
        float bossMaxHp = stats.GetMaxHP();
        if (bossCurrentHp < bossMaxHp / 2) // 보스의 HP가 50% 미만으로 떨어졌을 때 페이즈2 시작
            return true;
        else
            return false;
    }
}
