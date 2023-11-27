using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossState
{
    public BossAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {
    }

    private bool isAttack { get; set; } = false;
    private float timeToArrive = 17f;
    public int distanceOfDestination { get; set; } = 17;
    private float timer;
    private float delayTime = 1f;


    public int patternSelectNumber { get; set; } // 0 : BasicAttack , 1: RushAttack 
    // pattern one
    private int pattern_Two_Damage = 10; // temp Value 
    // pattern two
    public override void Enter()
    {
        initConditions();
        Debug.Log("PATTERN : " + patternSelectNumber);
    }
    public override void Exit()
    {
        //boss.agent.SetDestination(boss.transform.position);
        patternSelectNumber = -1;
    }
    public override void StateActionUpdate()
    {
        if (isAttack)
        {
            if(patternSelectNumber == 0)
                boss.StartCoroutine(BossAttackPattern_BasicAttack());
            if (patternSelectNumber == 1)
                boss.StartCoroutine(BossAttackPattern_Rush());
            isAttack = false;
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
        Debug.Log(distanceOfDestination);
        // 어차피 isAttack 시 한 번만 실행될 내용이라 그냥 update문에서 방향을 처리;
        Vector3 attackDirection = (boss.player.transform.position - boss.transform.position).normalized;
        // 보스가 플레이어가 있는 방향으로 길이를 distanceOfDestination 만큼 "대쉬 공격" 진행. 
        Vector3 destinationOfAttack = new Vector3(attackDirection.x * distanceOfDestination, 0f, attackDirection.z * distanceOfDestination) + boss.transform.position;
        boss.transform.LookAt(destinationOfAttack);
        Debug.Log("테스트문구");
        isAttack = false;
        while (Vector3.Distance(boss.transform.position, destinationOfAttack) > 0.5f)
        {
            timer += Time.deltaTime;
            
            boss.transform.position = Vector3.Lerp(boss.transform.position,destinationOfAttack,timer/timeToArrive); // 일종의 waypoint처럼 position은 lerp를 사용해 스무스하게 움직인다.
            if (boss.player.GetComponent<PlayerMovementHandler>().isCollisionWithBox)
            {
                boss.StartCoroutine(boss.player.GetComponent<PlayerMovementHandler>().KnockBack());
                boss.player.GetComponent<PlayerMovementHandler>().isCollisionWithBox = false;
                break;
            }
            yield return null;
        }
        bossStateMachine.ChangeState(boss.stiffnessState);     
    }
    IEnumerator BossAttackPattern_BasicAttack() 
    {       
        yield return boss.StartCoroutine(delayBeforeAttack());  // 공격을 하기 전 전조 증상을 플레이어에게 보여주어 플레이어가 대처할 수 있도록 함
        boss.bossAnimationHandler.OnBasicAttack();
        yield return new WaitForSeconds(0.6f); // 모션과 타이밍을 맞추기 위해 잠시 시간을 지연          
            if (boss.detectPlayer_AttackRange.getPlayerStatusForDamaged() != null) // 공격 범위 안에 플레이어가 존재할 때
            {
                if (!boss.player.GetComponent<PlayerMovementHandler>().getIsDodge())
                {
                    boss.detectPlayer_AttackRange.getPlayerStatusForDamaged().hpDown(pattern_Two_Damage);
                    boss.StartCoroutine(boss.player.GetComponent<PlayerMovementHandler>().KnockBack());
                }
            }       
        bossStateMachine.ChangeState(boss.stiffnessState);
        // 이제 여기는 효과 부여
    }

    IEnumerator delayBeforeAttack()
    {
        yield return new WaitForSeconds(delayTime);

    }

    private void initConditions()
    {
        isAttack = true;
        timer = 0f;
        boss.agent.SetDestination(boss.transform.position);
    }
}
