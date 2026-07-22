using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private bool isFlightAttack;
    // damage
    //private float pattern_Zero_Damage = 13f;
    private float pattern_One_Damage = 15f;
    //private float pattern_Two_Damage = 16f;


    // DropBomb Property
    private List<GameObject> shuffledFlightPoint;
    private List<GameObject> dropBombPoint = new List<GameObject>();
    private List<Vector3> setOfDivePosition = new List<Vector3>();
    private Dictionary<string, List<Vector3>> setOfTravelPointWhileDropBomb = new Dictionary<string, List<Vector3>>();
    private int currentIndex;
    private float timeToArrive = 500f;
    private float timeToArriveOfGlideRush = 220f;
    // Fireball Attack Property

    private delegate List<Vector3> AddList(string first, string second, string third);
    AddList addList;
    public BossFlightAttack(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {

    }
    public override void Enter()
    {
        Initialize();
        if (!isFlightAttack)
            bossPatternCheck_FlightAttack();
    }

    public override void Exit()
    {
        setOfTravelPointWhileDropBomb.Clear();
    }

    public override void StateActionUpdate()
    {
        if (!isFlightAttack)
            bossPatternCheck_FlightAttack();
    }

    public override void StateActionFixedUpdate()
    {

    }

    private void bossPatternCheck_FlightAttack()
    {
        if (patternSelectNumber == 0)
        {
            OnFlightFireBall().Forget();
            Debug.Log("pattern 0");
        }
        else if (patternSelectNumber == 1)
        {
            OnGlideRush().Forget();
            Debug.Log("pattern 1");
        }
        else if (patternSelectNumber == 2)
        {
            OnDropBomb().Forget();
            Debug.Log("pattern 2");
        }

    }
    private async UniTask OnFlightFireBall(CancellationToken ct = default)
    {
        isFlightAttack = true;

        for (int i = 0; i < 3; i++)
        {
            boss.transform.LookAt(boss.player.transform);
            boss.bossAnimationHandler.OnFireballAttack();

            await UniTask.WaitUntil(() =>
            boss.bossAnimationHandler.AnimationPlayingCheck(
                1, 0.95f, "Fly Fireball Shoot"), cancellationToken: ct
                );
            Debug.Log($"발사 {i} 회차");
            await UniTask.Delay(500, cancellationToken: ct);
        }
        isFlightAttack = false;
        bossStateMachine.ChangeState(boss.flyAroundState);
    }
    private async UniTask OnGlideRush(CancellationToken ct = default)
    {
        isFlightAttack = true;
        await UniTask.Yield(PlayerLoopTiming.Update, ct);

        float timer = 0f;
        boss.BossCollisionBoxActive(true);

        Vector3 directionToPlayer = boss.player.transform.position;
        Vector3 directionToMovePoint = boss.flyAroundState.moveToPoints[boss.flyAroundState.count].transform.position;

        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.RushAttack, false, boss);

        while (Vector3.Distance(boss.transform.position, directionToPlayer) > 0.2f)
        {
            timer += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(boss.transform.position, directionToPlayer, timer / timeToArriveOfGlideRush);

            if (boss.CheckPlayerCollisionToBoss(pattern_One_Damage)) break;

            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        boss.BossCollisionBoxActive(false);
        timer = 0f;

        await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: ct);

        while (Vector3.Distance(boss.transform.position, directionToMovePoint) > 0.05f)
        {
            timer += Time.deltaTime;
            boss.transform.LookAt(directionToMovePoint);
            boss.transform.position = Vector3.Slerp(boss.transform.position, directionToMovePoint, timer / timeToArriveOfGlideRush);

            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        bossStateMachine.ChangeState(boss.flyAroundState);
    }

    private async UniTask OnDropBomb(CancellationToken ct = default)
    {
        isFlightAttack = true;
        await UniTask.Yield(PlayerLoopTiming.Update, ct);

        var travelPoints = setOfTravelPointWhileDropBomb[shuffledFlightPoint[currentIndex].name];

        for (int i = 0; i < travelPoints.Count; i++)
        {
            float timer = 0f;
            Vector3 flyToPosition = travelPoints[i];

            // InvokeRepeating 대신 UniTask 병렬 실행
            var bombCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            OnDropBombLoop(bombCts.Token).Forget();

            while (Vector3.Distance(boss.transform.position, flyToPosition) > 0.2f)
            {
                timer += Time.deltaTime;
                boss.transform.LookAt(flyToPosition);
                boss.transform.position = Vector3.Slerp(boss.transform.position, flyToPosition, timer / timeToArrive);

                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            bombCts.Cancel();
            bombCts.Dispose();

            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        bossStateMachine.ChangeState(boss.flyAroundState);
    }

    private async UniTask OnDropBombLoop(CancellationToken ct = default)
    {
        while (true)
        {
            boss.InstanceAndDrobFirebomb();
            await UniTask.Delay(TimeSpan.FromMilliseconds(500), cancellationToken: ct);
        }
    }
    #region Non Use
    //IEnumerator bossFlightAttackPattern_Fireball()
    //{
    //    isFlightAttack = true;
    //    yield return null;
    //    for (int i = 0; i < 3; i++)
    //    {
    //        boss.transform.LookAt(boss.player.transform);
    //        boss.bossAnimationHandler.OnFireballAttack();
    //        yield return new WaitUntil(() => boss.bossAnimationHandler.AnimationPlayingCheck(1, 0.95f, "Fly Fireball Shoot"));
    //        Debug.Log($"발사 {i} 회차");
    //        yield return new WaitForSeconds(0.5f); // 시간을 지연시키지 않으면 다음 모션이 실행되기 전에 위에서 bool값이 true가 되버림
    //    }
    //    isFlightAttack = false;
    //    bossStateMachine.ChangeState(boss.flyAroundState);
    //}
    //IEnumerator bossFlightAttackPattern_GlideRush()
    //{
    //    isFlightAttack = true;
    //    yield return null;
    //    float timer = 0f;
    //    boss.BossCollisionBoxActive(true); // 콜리전 박스 키기
    //    Vector3 directionToPlayer = boss.player.transform.position;
    //    Vector3 directionToMovePoint = boss.flyAroundState.moveToPoints[boss.flyAroundState.count].transform.position;
    //    SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.RushAttack, false, boss);
    //    while (Vector3.Distance(boss.transform.position, directionToPlayer) > 0.2f)
    //    {
    //        timer += Time.deltaTime;
    //        boss.transform.position = Vector3.Lerp(boss.transform.position, directionToPlayer, timer / timeToArriveOfGlideRush); // 일종의 waypoint처럼 position은 lerp를 사용해 스무스하게 움직인다.
    //        if (boss.CheckPlayerCollisionToBoss(pattern_One_Damage)) break; // bool 값 체크하는 곳에서 이러는게 조금 그렇긴 함
    //        yield return null;
    //    }
    //    boss.BossCollisionBoxActive(false); // 끄기
    //    timer = 0f;
    //    yield return new WaitForSeconds(3f); // 딜타임
    //    while(Vector3.Distance(boss.transform.position, directionToMovePoint) > 0.05f)
    //    {
    //        timer += Time.deltaTime;
    //        boss.transform.LookAt(Vector3.Slerp(boss.transform.position, directionToMovePoint, 360f));
    //        boss.transform.position = Vector3.Slerp(boss.transform.position, directionToMovePoint, timer / timeToArriveOfGlideRush); // 일종의 waypoint처럼 position은 lerp를 사용해 스무스하게 움직인다.
    //        yield return null;
    //    }
    //    bossStateMachine.ChangeState(boss.flyAroundState);
    //}
    //IEnumerator bossFlightAttackPattern_DropBomb()
    //{
    //    isFlightAttack = true;
    //    yield return null;

    //    for (int i = 0; i < setOfTravelPointWhileDropBomb[shuffledFlightPoint[currentIndex].name].Count; i++)
    //    {
    //        float timer = 0f;
    //        Vector3 FlyToPosition = setOfTravelPointWhileDropBomb[shuffledFlightPoint[currentIndex].name][i];
    //        boss.InvokeRepeating("InstanceAndDrobFirebomb", 0f, 0.5f);
    //        while (Vector3.Distance(boss.transform.position, FlyToPosition) > 0.2f)
    //        {
    //            timer += Time.deltaTime;
    //            boss.transform.LookAt(Vector3.Slerp(boss.transform.position, FlyToPosition, 360f));
    //            boss.transform.position = Vector3.Slerp(boss.transform.position, FlyToPosition, timer / timeToArrive);
    //            yield return null;
    //        }
    //        boss.CancelInvoke("InstanceAndDrobFirebomb");
    //        yield return null;
    //    }

    //    bossStateMachine.ChangeState(boss.flyAroundState);
    //}
    #endregion
    private async UniTask DropBombLoop(CancellationToken ct)
    {
        while (true)
        {
            boss.InstanceAndDrobFirebomb();
            await UniTask.Delay(TimeSpan.FromMilliseconds(500), cancellationToken: ct);
        }
    }

    private void IntializeOrderToDropBombPoint()
    {
        for (int i = 1; i <= 8; i++)
        {
            dropBombPoint.Add(GameObject.Find($"point{i}"));
        }
    }

    private void SetDropBombPosition()
    {
        // set 1
        setOfTravelPointWhileDropBomb.Add("point1", Action("point4", "point6", "point1", AddPointToSetList));
        setOfTravelPointWhileDropBomb.Add("point4", Action("point1", "point6", "point4", AddPointToSetList));
        setOfTravelPointWhileDropBomb.Add("point6", Action("point4", "point1", "point6", AddPointToSetList));
        setOfTravelPointWhileDropBomb.Add("point2", Action("point4", "point7", "point2", AddPointToSetList));
        setOfTravelPointWhileDropBomb.Add("point7", Action("point4", "point2", "point7", AddPointToSetList));
        setOfTravelPointWhileDropBomb.Add("point3", Action("point5", "point8", "point3", AddPointToSetList));
        setOfTravelPointWhileDropBomb.Add("point5", Action("point3", "point8", "point5", AddPointToSetList));
        setOfTravelPointWhileDropBomb.Add("point8", Action("point3", "point5", "point8", AddPointToSetList));
    }
    private List<Vector3> AddPointToSetList(string first, string second, string third)
    {
        List<Vector3> divePos = new List<Vector3>();
        // 람다식 사용, List.Find() 를 사용하려면 Find() 의 매개변수가 predicate 즉 대체자의 자료형을 가지고 있는데 이는 람다식을 통해 이용 가능
        divePos.Add(shuffledFlightPoint.Find(x => x.name == first).transform.position);
        divePos.Add(shuffledFlightPoint.Find(x => x.name == second).transform.position);
        divePos.Add(shuffledFlightPoint.Find(x => x.name == third).transform.position);

        return divePos;
    }

    private List<Vector3> Action(string first, string second, string third, AddList addList)
    {
        return addList(first, second, third);
    }
    private void ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; i++)
        {
            random1 = UnityEngine.Random.Range(0, list.Count);
            random2 = UnityEngine.Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }
    }

    private void Initialize()
    {
        isFlightAttack = false;
        currentIndex = boss.flyAroundState.count;
        shuffledFlightPoint = boss.flightPoint;

        // GlideRush
        boss.bossCollisionBox.isCollisionWithPlayer = false; // 공격을 할 때마다 새로 콜리전을 체크.


        // DrobBomb Init
        setOfDivePosition.Clear();
        IntializeOrderToDropBombPoint();
        SetDropBombPosition();
    }
}
