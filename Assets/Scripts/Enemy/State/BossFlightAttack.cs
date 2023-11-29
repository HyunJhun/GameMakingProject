using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlightAttack : BossState
{
    /*
    1. 파이어볼 발사
    2. 날아가면서 불똥 투하(밝거나 시간 지나면 터짐)
    3. 가끔 날아가다가 하강해서 박치기
    4. 잠시 하강해서 휴식(딜타임)
     */
    public int patternSelectNumber { get; set; } = -1; // 0 : 파이어볼 , 1 : 활강 돌진 , 2 : 폭탄 투하
    private float readyTimeToAttack = 3f;
    private bool isFlightAttack;

    // DropBomb Property
    private List<GameObject> shuffledFlightPoint;
    private List<Vector3> setOfDivePosition = new List<Vector3>();
    private int currentIndex;
    private float timerToArrive = 500f;
    // Fireball Attack Property
    public BossFlightAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {

    }
    public override void Enter()
    {
        Debug.Log("패턴 넘버는? : " + patternSelectNumber);
        isFlightAttack = false;
        currentIndex = boss.flyAroundState.count;
        shuffledFlightPoint = boss.flightPoint;
        setOfDivePosition.Clear();
        /*
        for (int i = 0; i < 8; i++)
        {        
            Debug.Log("번호 = " + i + " 네임 : " + shuffledFlightPoint[i].name);
        }
        */

    }

    public override void Exit()
    {
        
    }

    public override void StateActionUpdate()
    {
        if(!isFlightAttack)
            bossPatternCheck_FlightAttack();
    }

    public override void StateActionFixedUpdate()
    {
        
    }

    private void bossPatternCheck_FlightAttack()
    {
        if(patternSelectNumber == 0)
        {
            boss.StartCoroutine(bossFlightAttackPattern_Fireball());
        }
        else if(patternSelectNumber == 1)
        {
            //boss.StartCoroutine(bossFlightAttackPattern_GlideRush());
            Debug.Log("pattern 1");
        }
        else if(patternSelectNumber == 2)
        {
            boss.StartCoroutine(bossFlightAttackPattern_DiveBomber());
            Debug.Log("pattern 2");
        }
        
    }
    
    IEnumerator bossFlightAttackPattern_Fireball()
    {
        isFlightAttack = true;
        yield return null;
        for (int i = 0; i < 3; i++)
        {
            boss.bossAnimationHandler.OnFireballAttack();
            yield return new WaitForSeconds(2f); // 모션이 충분히 나올 시간을 줌.
        }
        isFlightAttack = false;
        bossStateMachine.ChangeState(boss.flyAroundState);
        yield return null;
    }
    IEnumerator bossFlightAttackPattern_GlideRush()
    {
        yield return new WaitForSeconds(readyTimeToAttack);
        Debug.Log("GlideRush");
    }
    IEnumerator bossFlightAttackPattern_DiveBomber()
    {
        isFlightAttack = true;
        yield return null;
        

        if (shuffledFlightPoint[currentIndex].name == "point1")
        {   
            // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point4").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point6").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point1").transform.position);
        }
        else if (shuffledFlightPoint[currentIndex].name == "point4")
        {
            // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point1").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point6").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point4").transform.position);
        }
        else if (shuffledFlightPoint[currentIndex].name == "point6")
        {
            // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point4").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point1").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point6").transform.position);
        }


        else if (shuffledFlightPoint[currentIndex].name == "point2")
        {
            // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point4").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point7").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point2").transform.position);
        }
        else if (shuffledFlightPoint[currentIndex].name == "point7")
        {
            // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point4").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point2").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point7").transform.position);
        }


        else if (shuffledFlightPoint[currentIndex].name == "point3")
        {
            // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point5").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point8").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point3").transform.position);
        }
        else if (shuffledFlightPoint[currentIndex].name == "point5")
        {
            // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point3").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point8").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point5").transform.position);
        }
        else if (shuffledFlightPoint[currentIndex].name == "point8")
        {
            // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point3").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point5").transform.position);
            setOfDivePosition.Add(shuffledFlightPoint.Find(x => x.name == "point8").transform.position);
        }

        //yield return new WaitForSeconds(1f);
        for (int i = 0; i < setOfDivePosition.Count; i++)
        {
            float timer = 0f;
            Vector3 FlyToPosition = setOfDivePosition[i];
            boss.InvokeRepeating("InstanceAndDrobFirebomb", 0f, 0.5f);
            while (Vector3.Distance(boss.transform.position, FlyToPosition) > 0.2f)
            {
                timer += Time.deltaTime;
                boss.transform.LookAt(Vector3.Slerp(boss.transform.position, FlyToPosition, 360f));
                boss.transform.position = Vector3.Slerp(boss.transform.position, FlyToPosition, timer / timerToArrive);               
                yield return null;
            }
            boss.CancelInvoke("InstanceAndDrobFirebomb");
            yield return null;
        }

        bossStateMachine.ChangeState(boss.flyAroundState);
        yield return null;
    }

}
