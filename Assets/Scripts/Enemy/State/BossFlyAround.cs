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
        moveToPoints = boss.flightPoint;
        ShuffleList(moveToPoints);
        for(int i = 0; i< moveToPoints.Count; i++)
        {
            Debug.Log(moveToPoints[i]);
        }
        count = 0;
        timer = 0f;
        isMoving = false;
        Debug.Log("name : " + moveToPoints[count] + " isTrigger : " + moveToPoints[count].GetComponent<DetectBossWhileFlight>().isTriggered);
    }

    public override void Exit()
    {

    }

    public override void StateActionUpdate()
    {
        if(!isMoving)
            boss.StartCoroutine(FlyToPoint());
        Debug.Log("카운트는 ? : " + count);
    }

    public override void StateActionFixedUpdate()
    {
        
    }
    IEnumerator FlyToPoint()
    {
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
        yield return new WaitForSeconds(1f);
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
}
