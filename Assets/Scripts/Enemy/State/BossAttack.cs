using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossState
{
    public BossAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {
    }

    private bool isAttack { get; set; } = false;
    private float timeToArrive = 2f;
    private int distanceOfDestination = 7;
    private float bossMoveSpeedToAttack = 10f;

    private List<IEnumerator> BossAttackPaterns;
    public override void Enter()
    {
        Debug.Log("AttackEnter");  
        for(int i = 0;i<1;i++)
        {
            BossAttackPaterns[i] = BossAttackPattern_One();
        }
    }
    public override void Exit()
    {
        Debug.Log("AttackExit");
    }
    public override void StateActionUpdate()
    {

        if (!isAttack)
        {
            boss.StartCoroutine(BossAttackPattern_One());
        }
        // 공격을 하는 동시에 상대방을 향해 전진하며 가까이 다가가 상대가 움직였을 때를 방지한다. 
    }

    public override void StateActionFixedUpdate()
    {
    }
    IEnumerator BossAttackPattern_One()
    {
        float timer = 0f;
        Vector3 attackDirection = (boss.player.transform.position - boss.transform.position).normalized; // 어차피 isAttack 시 한 번만 실행될 내용이라 그냥 update문에서 방향을 처리;
        Vector3 destinationOfAttack = boss.transform.position + (boss.transform.forward * distanceOfDestination); // 보스가 보고있는 방향에서 길이를 distanceOfDestination 만큼 이동하며 "박치기" 진행.
        isAttack = true;
        while (Vector3.Distance(boss.transform.position, attackDirection + destinationOfAttack) > 0.1f)
        {
            timer += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(boss.transform.position, attackDirection + destinationOfAttack,timer/timeToArrive); // 일종의 waypoint처럼 position은 lerp를 사용해 스무스하게 움직인다.
            Debug.Log("이동중 : " + (attackDirection + destinationOfAttack));
            yield return null;
        }     
        isAttack = false;
        Debug.Log("이동 완");
        bossStateMachine.ChangeState(boss.stiffnessState);
        
    }

}
