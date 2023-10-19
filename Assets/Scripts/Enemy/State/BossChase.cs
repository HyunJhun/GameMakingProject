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
    

    private float timer;
    public override void Enter()
    {
        Debug.Log("ChaseEnter");
        // 만약 버그등으로 인해 상태가 잘못 들어왔을시를 방지
        if(!boss.detectPlayer.isDetectPlayer) 
        {
            bossStateMachine.ChangeState(boss.idleState);
            return;
        }
        timer = 0f;
    }
    public override void Exit()
    {
        Debug.Log("ChaseExit");
    }
    public override void StateActionUpdate()
    {
        
        // 보스와 플레이어 사이의 방향성 확보 => 추격 시 움직이는 방향
        Vector3 direction = boss.gameObject.transform.position - boss.player.transform.position;
        // 현재 탐지 범위의 반지름은 7.5 ... distance가 7.5가 최대치여야함
        float distance = Mathf.Abs(Vector3.Distance(boss.gameObject.transform.position, boss.player.transform.position));
        // 보스 회전
        Vector3 forward = Vector3.Slerp(direction, boss.transform.forward,
                boss.rotationSpeed * Time.deltaTime / Vector3.Angle(direction, boss.transform.forward));     

        if (distance <= chaseRange) // 추격 범위 설정
        {
            //boss.transform.LookAt(boss.transform.position - forward);
            //boss.GetComponent<Rigidbody>().transform.Translate(direction * boss.moveSpeed * Time.deltaTime);
            boss.agent.destination = boss.player.transform.position;
            timer = 0f;
            if (distance < radius - 3f) // 공격 사정 거리
            {
                bossStateMachine.ChangeState(boss.attackState);
                return;
            }
        }
        else if(distance > chaseRange) // 추격 범위를 벗어나면
        {
            if(timer < 3f) // 플레이어를 놓쳐서 잠시 대기하여 추격 범위에 플레이어가 다시 들어오는지 체크하는 역할
            {
                timer += Time.deltaTime;
                if (timer % 1 == 0)
                    Debug.Log(timer);
            }
            else // 만약 3초 동안 플레이어가 추격 범위에 들어오지 않았을 경우 시작 위치로 복귀
            {
                timer = 0f;
                bossStateMachine.ChangeState(boss.backState);
            }
        }

        
    }
}
