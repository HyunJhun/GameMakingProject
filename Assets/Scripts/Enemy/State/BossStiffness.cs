using UnityEngine;

public class BossStiffness : BossState
{
    public BossStiffness(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {
    }


    private float timer = 0f;
    private float bugTimer = 0f;
    // 각 previousState에 따른 경직 시간 변수들
    private float stiffTimeOfOutOfRangeByChasing = 3f;
    private float stiffTimeOfOutOfRangeByAttack= 1.5f;

    public override void Enter()
    {
        Debug.Log("StiffnesseEnter");
        boss.transform.position = boss.transform.position; // 일단 경직이라는 개념 자체가 멈춰있는 상태이기 떄문에 멈춤
        timer = 0f;
        bugTimer = 0f;
    }
    public override void Exit()
    {
        Debug.Log("StiffnessExit");
    }
    public override void StateActionUpdate()
    {
        ActionUpdateByPreviousState();
        if(timer < 5f)
        {
            bugTimer += Time.deltaTime;
        }
        else
        {
            Debug.Log("버그 수정 - 경직");
            bossStateMachine.ChangeState(bossStateMachine.previousState);
        }
    }

    public override void StateActionFixedUpdate()
    {

    }

    private void ActionUpdateByPreviousState()
    {
        // 후에 Swtich문으로 바꿔야함 **** , if문으로 처리한건 일단 임시.
        if (bossStateMachine.previousState == boss.chaseState)
        {
            if (timer < stiffTimeOfOutOfRangeByChasing) // 플레이어를 놓쳐서 잠시 대기하여 추격 범위에 플레이어가 다시 들어오는지 체크하는 역할
            {
                timer += Time.deltaTime;
                Debug.Log("추격 범위 밖");
            }
            else // 만약 3초 동안 플레이어가 추격 범위에 들어오지 않았을 경우 시작 위치로 복귀
            {
                timer = 0f;
                bossStateMachine.ChangeState(boss.backState);
                return;
            }
            return;
        }
        else if (bossStateMachine.previousState == boss.attackState) // 공격을 한 이후 정해진 시간만큼 경직이 일어난다.
        {
            if (timer < stiffTimeOfOutOfRangeByAttack) 
            {
                timer += Time.deltaTime;
                Debug.Log("추격 범위 밖");
            }
            else 
            {
                timer = 0f;
                bossStateMachine.ChangeState(boss.chaseState);
                return;
            }
            return;
        }
        else if (bossStateMachine.previousState == boss.chaseState)
        {
            if (timer < stiffTimeOfOutOfRangeByAttack) // 플레이어를 놓쳐서 잠시 대기하여 추격 범위에 플레이어가 다시 들어오는지 체크하는 역할
            {
                timer += Time.deltaTime;
                Debug.Log("추격 범위 밖");
            }
            else // 만약 3초 동안 플레이어가 추격 범위에 들어오지 않았을 경우 시작 위치로 복귀
            {
                timer = 0f;
                bossStateMachine.ChangeState(boss.chaseState);
                return;
            }
            return;
        }
    }
}
