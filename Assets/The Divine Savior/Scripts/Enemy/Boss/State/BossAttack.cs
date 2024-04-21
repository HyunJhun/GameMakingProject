using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossState
{
    public BossAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {
    }
    private bool _isAttack = false;
    private float timeToArrive = 17f;
    public int distanceOfDestination { get; set; } = 17;
    private float timer;
    private float delayTime = 1f;


    public int patternSelectNumber { get; set; } // 0 : BasicAttack , 1: RushAttack 
    // pattern one
    private float pattern_One_Damage = 17f;
    private float pattern_Two_Damage = 10f; // temp Value 
    // pattern two
    public override void Enter()
    {
        initConditions();
        boss.isAttack = true;
        Debug.Log("PATTERN : " + patternSelectNumber);
    }
    public override void Exit()
    {
        //boss.agent.SetDestination(boss.transform.position);
        boss.isAttack = false;
        patternSelectNumber = -1;
    }
    public override void StateActionUpdate()
    {
        if (boss.coolTime_RushAttack <= 3f) boss.coolTime_RushAttack += Time.deltaTime;
        if (boss.coolTime_BreathAttack <= 10f) boss.coolTime_BreathAttack += Time.deltaTime;
        if (_isAttack)
        {
            if(patternSelectNumber == 0)
                boss.StartCoroutine(BossAttackPattern_BasicAttack());
            if (patternSelectNumber == 1)
                boss.StartCoroutine(BossAttackPattern_Rush());
            if(patternSelectNumber == 2)
                boss.StartCoroutine(BossAttackPattern_FireBreath());
            _isAttack = false;
        }
        // attack 이 연속적으로 일어나면 일시적으로 attack 상태에서 상태변환이 안일어남
        // 그러면 일단 만약 몇초동안 움직임이 없다면 다시 추격 상태로 강제로 돌려야 하는 방법이 있을수도 있음.
    }
    public override void StateActionFixedUpdate()
    {
        
    }
    IEnumerator BossAttackPattern_Rush()
    {
        yield return boss.StartCoroutine(delayBeforeAttack());
        distanceOfDestination = boss.ShotRay(distanceOfDestination);
        boss.BossCollisionBoxActive(true);
        // 어차피 isAttack 시 한 번만 실행될 내용이라 그냥 update문에서 방향을 처리;
        Vector3 attackDirection = (boss.player.transform.position - boss.transform.position).normalized;
        // 보스가 플레이어가 있는 방향으로 길이를 distanceOfDestination 만큼 "대쉬 공격" 진행. 
        Vector3 destinationOfAttack = new Vector3(attackDirection.x * distanceOfDestination, 0f, attackDirection.z * distanceOfDestination) + boss.transform.position;
        boss.transform.LookAt(destinationOfAttack);
        _isAttack = false;
        boss.bossParticleManager.BossAttackParticleInstance(1);
        while (Vector3.Distance(boss.transform.position, destinationOfAttack) > 0.2f)
        {
            timer += Time.deltaTime;        
            boss.transform.position = Vector3.Lerp(boss.transform.position,destinationOfAttack,timer/timeToArrive); // 일종의 waypoint처럼 position은 lerp를 사용해 스무스하게 움직인다.
            if (boss.CheckPlayerCollisionToBoss(pattern_One_Damage)) break; // 만약 플레이어가 보스와 충돌했을 경우 보스의 이동이 멈추고 while문을 탈출하고 공격을 중지.
            yield return null;
        }
        boss.BossCollisionBoxActive(false);
        bossStateMachine.ChangeState(boss.stiffnessState);     
    }
    IEnumerator BossAttackPattern_BasicAttack() 
    {       
        yield return boss.StartCoroutine(delayBeforeAttack());  // 공격을 하기 전 전조 증상을 플레이어에게 보여주어 플레이어가 대처할 수 있도록 함
        boss.bossAnimationHandler.OnBasicAttack();
        yield return new WaitForSeconds(0.7f); // 모션과 타이밍을 맞추기 위해 잠시 시간을 지연          
        CheckIsPlayerInRangeOfAttackRangeForBasicAttackPattern(); // 공격범위 안에 플레이어가 존재하는지 체크.
        bossStateMachine.ChangeState(boss.stiffnessState);
    }
    IEnumerator BossAttackPattern_FireBreath()
    {
        yield return boss.StartCoroutine(delayBeforeAttack());
        float sign = Random.Range(0, 1) == 0 ? 1f : -1f;
        Quaternion targetRotation = Quaternion.Euler(boss.transform.rotation.eulerAngles.x, boss.transform.rotation.eulerAngles.y + 120f * sign, boss.transform.rotation.eulerAngles.z);
        GameObject breathParticle;

        float angleDifference = Quaternion.Angle(boss.transform.rotation, targetRotation);

        boss.transform.LookAt(boss.player.transform);

        boss.bossAnimationHandler.GetBossAnimator().SetTrigger("BreathStart");

        breathParticle = boss.InstanceFireBraath();

        while (angleDifference > 5f) 
        {
            boss.bossAnimationHandler.GetBossAnimator().SetTrigger("Breathing");
            boss.transform.rotation = Quaternion.Lerp(boss.transform.rotation, targetRotation, Time.deltaTime / 2f);
            angleDifference = Quaternion.Angle(boss.transform.rotation, targetRotation);
            yield return null;
        }
        boss.bossAnimationHandler.GetBossAnimator().SetTrigger("BreathEnd");
        GameObject.Destroy(breathParticle, 0.2f);
        bossStateMachine.ChangeState(boss.stiffnessState);

    }
    IEnumerator delayBeforeAttack()
    {
        if (patternSelectNumber == 0)
        {
            float timer = 0f;
            GameObject enterParticle = boss.bossParticleManager.BossAttackEnterParticleInstance(0);
            while (enterParticle.transform.localScale.x < 6 && enterParticle.transform.localScale.z < 6)
            {

                enterParticle.transform.localScale = Vector3.Lerp(enterParticle.transform.localScale, new Vector3(6f, 0f, 6f), timer / delayTime / 2f);
                timer += Time.deltaTime;
                yield return null;
            }
            GameObject.Destroy(enterParticle, 1f);
        }
        yield return new WaitForSeconds(delayTime);
    }

    private void initConditions()
    {
        _isAttack = true;
        timer = 0f;
        boss.agent.SetDestination(boss.transform.position);
        boss.bossCollisionBox.isCollisionWithPlayer = false;
    }

    private void CheckIsPlayerInRangeOfAttackRangeForBasicAttackPattern()
    {
        if (boss.detectPlayer_AttackRange.getPlayerStatusForDamaged() != null) // 공격 범위 안에 플레이어가 존재할 때
        {
            if (!boss.CheckPlayerDodge())
            {
                boss.detectPlayer_AttackRange.getPlayerStatusForDamaged().hpDown(pattern_Two_Damage - boss.player.GetComponent<Status>().GetArmor());
                boss.player.GetComponent<Player>().b_IsHit = true;
                boss.player.GetComponent<Player>().b_IsKnockback = true;
            }
        }
    }
    
}
