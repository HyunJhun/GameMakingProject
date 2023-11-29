using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlyAround : BossState
{

    public BossFlyAround(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {

    }
    private List<GameObject> moveToPoints;
    private int count;
    private float timer;
    private float timerToArrive = 500f;
    private bool isMoving;

    public override void Enter()
    {
        if (bossStateMachine.previousState != boss.flightAttackState)
        {
            InitializeProperties();
            ShuffleList(moveToPoints);
        }
    }
    public override void Exit()
    {

    }

    public override void StateActionUpdate()
    {
        if(!isMoving)
            boss.StartCoroutine(FlyToPoint());
    }

    public override void StateActionFixedUpdate()
    {
        
    }
    IEnumerator FlyToPoint()
    {
        yield return null;
        isMoving = true;
        //yield return new WaitForSeconds(1f);
        Debug.Log("과연 ? : " + count);
        Vector3 FlyToPosition = moveToPoints[count].transform.position;

        while (Vector3.Distance(boss.transform.position,FlyToPosition) > 0.2f)
        {
            timer += Time.deltaTime;
            boss.transform.LookAt(Vector3.Slerp(boss.transform.position,FlyToPosition,360f));
            boss.transform.position = Vector3.Slerp(boss.transform.position, FlyToPosition, timer / timerToArrive);
            Debug.Log("횟수 체크");
            yield return null;
        }
        count++;
        timer = 0f;
        boss.transform.LookAt(Vector3.Slerp(boss.transform.position, boss.player.transform.position, 360f));
        if (count != moveToPoints.Count) // 만약 모든 지점을 들리지 않았다면
            isMoving = false;
        else
            bossStateMachine.ChangeState(boss.landingState); // 만약 모든 지점을 들렸다면 공중 패턴이 끝난 것이므로 다시 지상패턴으로 가기 위해 땅으로 랜딩한다.

        PatternSelectToFlightAttack();
        yield return new WaitForSeconds(1f);
        
    }

    private void PatternSelectToFlightAttack()
    {
        /*
        1. 파이어볼 발사
        2. 날아가면서 불똥 투하(밝거나 시간 지나면 터짐)
        3. 가끔 날아가다가 하강해서 박치기
        4. 잠시 하강해서 휴식(딜타임)
        */
        int randomNumber = -1;
        // 각 패턴은 확률별로 함
        if (Vector3.Distance(moveToPoints[count].transform.position, moveToPoints[count + 1].transform.position) > 21f) // 일직선 상 가까운 두 지점
        {
            randomNumber = Random.Range(0, 100);
            // 파이어볼 or 활강 돌진
            if (randomNumber <= 100) // 70% 확률로 파이어 볼 발사
            {
                boss.flightAttackState.patternSelectNumber = 0;
                bossStateMachine.ChangeState(boss.flightAttackState);
                return;
            }
            else
            {
                boss.flightAttackState.patternSelectNumber = 1;
                bossStateMachine.ChangeState(boss.flightAttackState);
                return;
            }
        }
        else if (Vector3.Distance(moveToPoints[count].transform.position, moveToPoints[count + 1].transform.position) > 41f) // 일직선 상 먼 두 지점
        {
            randomNumber = Random.Range(0, 100);
            //폭탄 투하 or 파이어볼 or 활강돌진
            if (randomNumber <= 100) // 70% 확률로 파이어 볼 발사
            {
                boss.flightAttackState.patternSelectNumber = 0;
                bossStateMachine.ChangeState(boss.flightAttackState);
                return;
            }
            else if(70 < randomNumber && randomNumber <= 95) // 25% 확률로 활강 돌진
            {
                boss.flightAttackState.patternSelectNumber = 1;
                bossStateMachine.ChangeState(boss.flightAttackState);
                return;
            }
            else if(randomNumber > 95) // 폭탄 투하
            {
                boss.flightAttackState.patternSelectNumber = 2;
                bossStateMachine.ChangeState(boss.flightAttackState);
                return;
            }
        }
        else if (Vector3.Distance(moveToPoints[count].transform.position, moveToPoints[count + 1].transform.position) > 55f) // 대각선 가장 먼 지점
        {   // 폭탄 투하 확정
            if (randomNumber <= 100) // 70% 확률로 파이어 볼 발사
            {
                boss.flightAttackState.patternSelectNumber = 0;
                bossStateMachine.ChangeState(boss.flightAttackState);
                return;
            }
            else
                boss.flightAttackState.patternSelectNumber = 2;
            
        }
        Debug.Log("공중 공격 횟수 체크");
    }
    private void ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }
    }

    private void InitializeProperties()
    {
        moveToPoints = boss.flightPoint;
        count = 0;
        timer = 0f;
        isMoving = false;

    }
}
