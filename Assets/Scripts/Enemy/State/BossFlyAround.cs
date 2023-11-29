using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlyAround : BossState
{

    public BossFlyAround(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {

    }
    public List<GameObject> moveToPoints;
    public int count { get; set; }
    private float timer;
    private float timerToArrive = 500f;
    private bool isMoving;
    private bool isFly = false;

    public override void Enter()
    {
        if (!isFly)
        {
            InitializeProperties();
            ShuffleList(moveToPoints);
        }
    }
    public override void Exit()
    {
        if (isMoving) isMoving = false;   
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
        isMoving = true;
        yield return null;
        
        timer = 0f;
        //yield return new WaitForSeconds(1f);
        Vector3 FlyToPosition = moveToPoints[count].transform.position;
        while (Vector3.Distance(boss.transform.position,FlyToPosition) > 0.2f)
        {
            timer += Time.deltaTime;
            boss.transform.LookAt(Vector3.Slerp(boss.transform.position,FlyToPosition,360f));
            boss.transform.position = Vector3.Slerp(boss.transform.position, FlyToPosition, timer / timerToArrive);
            yield return null;
        }
        boss.transform.LookAt(Vector3.Slerp(boss.transform.position, boss.player.transform.position, 360f));         
        yield return new WaitForSeconds(1f);
        PatternSelectToFlightAttackAndChangeState();
        count++;

        if (count < moveToPoints.Count) // 만약 모든 지점을 들리지 않았다면
            isMoving = false;
        else
            bossStateMachine.ChangeState(boss.landingState); // 만약 모든 지점을 들렸다면 공중 패턴이 끝난 것이므로 다시 지상패턴으로 가기 위해 땅으로 랜딩한다.
    }

    private void PatternSelectToFlightAttackAndChangeState()
    {
        if (count == 0)
        {
            patternSelectByPercentage(0);
        }
        else
        {
            patternSelectByDistance();
        }
        Debug.Log("공중 공격 횟수 체크");
    }

    private bool checkDistanceBetweenTwoPoint()
    {
        return Vector3.Distance(moveToPoints[count].transform.position, moveToPoints[count - 1].transform.position) < 21f;
    }
    private bool checkDistanceBetweenTwoPoint(float distance)
    {
        return Vector3.Distance(moveToPoints[count].transform.position, moveToPoints[count - 1].transform.position) > distance;
    }
    private void patternSelectByDistance()
    {
        if (checkDistanceBetweenTwoPoint()) // 일직선 가장 가까운 두 지점
        {
            patternSelectByPercentage(0);
        }
        else if (checkDistanceBetweenTwoPoint(21f)) // 대각선 가장 가까운 두 지점
        {
            patternSelectByPercentage(0);
        }
        else if (checkDistanceBetweenTwoPoint(41f)) // 일직선 가장 먼 두 지점
        {
            patternSelectByPercentage(1);
        }
        else if (checkDistanceBetweenTwoPoint(55f)) // 대각선 가장 먼 두 지점
        {
            patternSelectByPercentage(2);
        }
    }

    private void patternSelectByPercentage(int numberToSelect)
    {
        int randomNumber = Random.Range(0, 100);

        if (numberToSelect == 0) // 일직선 상 가까운 두 지점
        {

            if(randomNumber <= 70) // 70% 확률로 파이어 볼 발사
            {
                patternSelectAndChangeState(0);
            }
            else
            {
                patternSelectAndChangeState(2);
            }

        }
        else if(numberToSelect == 1) // 일직선 상 먼 두 지점
        {

            //폭탄 투하 or 파이어볼 or 활강돌진
            if (randomNumber <= 60) // 70% 확률로 파이어 볼 발사
            {
                patternSelectAndChangeState(0);
            }
            else if (60 < randomNumber && randomNumber <= 95) // 25% 확률로 활강 돌진
            {
                patternSelectAndChangeState(2);
            }
            else if (randomNumber > 95) // 폭탄 투하
            {
                patternSelectAndChangeState(2);
            }

        }
        else if (numberToSelect == 2) // 대각선 가장 먼 지점
        {

            if (randomNumber <= 20) // 70% 확률로 파이어 볼 발사
            {
                patternSelectAndChangeState(0);
            }
            else
                patternSelectAndChangeState(2);

        }
    }
    private void patternSelectAndChangeState(int patternSelectNumber)
    {
        boss.flightAttackState.patternSelectNumber = patternSelectNumber;
        bossStateMachine.ChangeState(boss.flightAttackState);
        return;
    }

    private void ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; i++)
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
        isFly = true;

    }
}
