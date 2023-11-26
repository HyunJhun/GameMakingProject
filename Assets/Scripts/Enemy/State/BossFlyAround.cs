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
    private float timerToArrive = 20f;
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
        Debug.Log("name : " + moveToPoints[count] + " isTrigger : " + moveToPoints[count].GetComponent<DetectBossWhileFlight>().isTriggered);
    }

    public override void Exit()
    {

    }

    public override void StateActionUpdate()
    {
        if(timer == 0f)
            boss.StartCoroutine(FlyToPoint());
    }

    public override void StateActionFixedUpdate()
    {
        
    }
    IEnumerator FlyToPoint()
    {
        Vector3 FlyToPosition = moveToPoints[count].transform.position;
        timer += Time.deltaTime;
        //bool isTrigger = moveToPoints[count].GetComponent<DetectBossWhileFlight>().isTriggered;
        while (Vector3.Distance(boss.transform.position,FlyToPosition) < 0.2f)
        { 
            boss.transform.position = Vector3.Slerp(boss.transform.position, FlyToPosition, timer / timerToArrive);
            moveToPoints.Remove(moveToPoints[count]);
        }
        count++;
        timer = 0f;
        yield return null;
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
