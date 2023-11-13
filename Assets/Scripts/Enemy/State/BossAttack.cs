using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossState
{
    public BossAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {
    }

    private List<IEnumerator> bossPatternList;

    private bool isAttack { get; set; } = false;
    private float timeToArrive = 12f;
    public int distanceOfDestination { get; set; } = 8;
    private float timer;
    private float delayTime = 1f;

    // pattern one
    private int pattern_Two_Damage = 10; // temp Value 
    // pattern two
    public override void Enter()
    {
        isAttack = true;
        timer = 0f;
        bossPatternList[0] = BossAttackPattern_One();
        bossPatternList[1] = BossAttackPattern_Two();
    }
    public override void Exit()
    {
        boss.agent.SetDestination(boss.transform.position);
        isAttack = true;
    }
    public override void StateActionUpdate()
    {
        if (isAttack)
        {
            boss.StartCoroutine(BossAttackPattern_Two());
            isAttack = false;
        }    
        // attack 이 연속적으로 일어나면 일시적으로 attack 상태에서 상태변환이 안일어남
        // 그러면 일단 만약 몇초동안 움직임이 없다면 다시 추격 상태로 강제로 돌려야 하는 방법이 있을수도 있음.
    }
    public override void StateActionFixedUpdate()
    {
        
    }
    IEnumerator BossAttackPattern_One()
    {
        yield return boss.StartCoroutine(delayBeforeAttack());
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
            yield return null;
        }
        bossStateMachine.ChangeState(boss.stiffnessState);
        
    }
    IEnumerator BossAttackPattern_Two()
    {       
        yield return boss.StartCoroutine(delayBeforeAttack());  // 공격을 하기 전 전조 증상을 플레이어에게 보여주어 플레이어가 대처할 수 있도록 함
        boss.detectPlayer_AttackRange.getPlayerStatusForDamaged().hpDown(pattern_Two_Damage);
        boss.StartCoroutine(boss.player.GetComponent<PlayerMovementHandler>().KnockBack());     
        bossStateMachine.ChangeState(boss.stiffnessState);
        // 이제 여기는 효과 부여
    }

    IEnumerator delayBeforeAttack()
    {
        yield return new WaitForSeconds(delayTime);

    }
}
