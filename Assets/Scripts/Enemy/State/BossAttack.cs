using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossState
{
    public BossAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {
    }

    private bool isAttack { get; set; } = false;
    private float timeToArrive = 12f;
    private int distanceOfDestination = 7;
    private float timer;
    // boss trasnform 에 관련한 변수
    private float turnSpeed = 360f;
    private float bossMoveSpeedToAttack = 10f;

    private List<IEnumerator> BossAttackPaterns;
    public override void Enter()
    {
        Debug.Log("AttackEnter");
        isAttack = true;
        timer = 0f;
    }
    public override void Exit()
    {
        boss.agent.SetDestination(boss.transform.position);
        isAttack = true;
        Debug.Log("공격을 한 후 보스의 공격 상태 : " + isAttack);
        Debug.Log("AttackExit");
    }
    public override void StateActionUpdate()
    {

        if (isAttack)
        {
            boss.StartCoroutine(BossAttackPattern_One());
            isAttack = false;
            Debug.Log("업데이트문");
        }    
        Debug.Log("현재 보스의 공격 상태 : " + isAttack);
        // attack 이 연속적으로 일어나면 일시적으로 attack 상태에서 상태변환이 안일어남
        // 그러면 일단 만약 몇초동안 움직임이 없다면 다시 추격 상태로 강제로 돌려야 하는 방법이 있을수도 있음.
    }

    public override void StateActionFixedUpdate()
    {
    }
    IEnumerator BossAttackPattern_One()
    {
        Vector3 attackDirection = (boss.player.transform.position - boss.transform.position).normalized; // 어차피 isAttack 시 한 번만 실행될 내용이라 그냥 update문에서 방향을 처리;
        Vector3 destinationOfAttack = boss.transform.position + (attackDirection * distanceOfDestination); // 보스가 플레이어가 있는 방향으로 길이를 distanceOfDestination 만큼 "대쉬 공격" 진행.
        boss.transform.LookAt(Vector3.Slerp(boss.transform.position,destinationOfAttack,turnSpeed));
        isAttack = false;
        Debug.Log("패턴 들어가고나서");
        while (Vector3.Distance(boss.transform.position, destinationOfAttack) > 0.1f)
        {
            timer += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(boss.transform.position,destinationOfAttack,timer/timeToArrive); // 일종의 waypoint처럼 position은 lerp를 사용해 스무스하게 움직인다.
            yield return null;
        }
        Debug.Log("이동 완");
        bossStateMachine.ChangeState(boss.stiffnessState);
        
    }
   
}
