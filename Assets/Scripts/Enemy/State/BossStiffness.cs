using UnityEngine;

public class BossStiffness : BossState
{
    public BossStiffness(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {
    }


    private float timer = 0f;
    // 각 previousState에 따른 경직 시간 변수들
    private float stiffTimeOfOutOfRangeByChasing = 3f;
    private float stiffTimeOfOutOfRangeByAttack= 1.5f;
    private float stiffTimeOfAfterLanding = 5f;

    public override void Enter()
    {
        timer = 0f;
        boss.agent.SetDestination(boss.transform.position);
    }
    public override void Exit()
    {
    }
    public override void StateActionUpdate()
    {
        ActionUpdateByPreviousState();

    }

    public override void StateActionFixedUpdate()
    {

    }
    private void waitForNextAction(float waitingTime,BossState nextState)
    {
        if (timer < waitingTime) // 플레이어를 놓쳐서 잠시 대기하여 추격 범위에 플레이어가 다시 들어오는지 체크하는 역할
        {
            timer += Time.deltaTime;
            Debug.Log("추격 범위 밖");
        }
        else // 만약 3초 동안 플레이어가 추격 범위에 들어오지 않았을 경우 시작 위치로 복귀
        {
            timer = 0f;
            bossStateMachine.ChangeState(nextState);
            return;
        }
        return;
    }
    private void ActionUpdateByPreviousState()
    {
        // 후에 Swtich문으로 바꿔야함 **** , if문으로 처리한건 일단 임시.

        if (bossStateMachine.previousState == boss.chaseState)
        {
            waitForNextAction(stiffTimeOfOutOfRangeByChasing, boss.backState);
        }
        else if (bossStateMachine.previousState == boss.attackState) // 공격을 한 이후 정해진 시간만큼 경직이 일어난다.
        {
            waitForNextAction(stiffTimeOfOutOfRangeByAttack, boss.chaseState);
        }
        else if (bossStateMachine.previousState == boss.chaseState)
        {
            waitForNextAction(stiffTimeOfOutOfRangeByAttack, boss.chaseState);
        }
        else if (bossStateMachine.previousState == boss.stiffnessState) // 버그 발생 방지
        {
            waitForNextAction(stiffTimeOfOutOfRangeByAttack, boss.chaseState);
        }
        else if (bossStateMachine.previousState == boss.landingState)
        {
            waitForNextAction(stiffTimeOfAfterLanding, boss.chaseState); // 랜딩 후에는 잠시의 딜타임 (5초)
        }
    }
}
