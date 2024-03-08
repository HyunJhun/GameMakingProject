using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyState
{
    public EnemyPatrol(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    // Local Var
    private Vector3 currentPosition;
    private RaycastHit hitObject;
    private Vector3 startPos;
    Vector3 ve;
    private enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        ForwardLeft,
        ForwardRight,
        BackwardLeft,
        BackwardRight
    }

    private List<Direction> movableDirectionList = new List<Direction>();
    private Dictionary<Direction, Vector3> directionDictionary = new Dictionary<Direction, Vector3>();

    public override void Enter()
    {
        OnInitialize();
        OnPatrol();
        
    }

    public override void StateActionUpdate()
    {
        currentPosition = enemy.transform.position;

        if (Vector3.Distance(currentPosition,enemy.GetInitPosition()) > 20) // 초기 스폰 위치에서 너무 벗어나게 되면 다시 돌아오도록 설정. 이렇게 안하면 특정 영역에 머물지 못함
        {
            enemyStateMachine.ChangeState(enemy.returnState);
            return;
        }
        if (enemy.GetDetectPlayerRange().isDetectPlayer)
        {
            enemyStateMachine.ChangeState(enemy.detectState);
            return;
        }

        if (Vector3.Distance(enemy.transform.position,enemy.GetEnemyNavMeshAgent().destination) < 1f)
        {
            enemyStateMachine.ChangeState(enemy.idleState);
            return;
        }
        else
        {
            Debug.DrawRay(startPos,ve * enemy.f_patrolLength, Color.blue);
        }

    }
    public override void StateActionFixedUpdate()
    {
        base.StateActionFixedUpdate();
    }

    public override void Exit()
    {
        enemy.b_isPatrol = false;
        movableDirectionList.Clear();
        directionDictionary.Clear();
    }

    private void OnInitialize()
    {
        enemy.b_isPatrol = true;
        startPos = enemy.transform.position;
        enemy.GetEnemyNavMeshAgent().stoppingDistance = enemy.f_patrolStopingDistance;

        // Direction Init
        directionDictionary.Add(Direction.Forward,enemy.transform.forward);
        directionDictionary.Add(Direction.Backward ,- enemy.transform.forward);
        directionDictionary.Add(Direction.Right,enemy.transform.right);
        directionDictionary.Add(Direction.Left,- enemy.transform.right);
        directionDictionary.Add(Direction.ForwardRight,(enemy.transform.forward + enemy.transform.right).normalized);
        directionDictionary.Add(Direction.BackwardLeft,- (enemy.transform.forward + enemy.transform.right).normalized);
        directionDictionary.Add(Direction.ForwardLeft,(enemy.transform.forward + -enemy.transform.right).normalized);
        directionDictionary.Add(Direction.BackwardRight ,- (enemy.transform.forward + -enemy.transform.right).normalized);
    }

    private void CheckEightDirectionForPatrol()
    {
        foreach(KeyValuePair<Direction,Vector3> pair in directionDictionary)
        {
            if (!Physics.Raycast(enemy.transform.position, pair.Value * enemy.f_patrolLength, out hitObject, 7, enemy.GetWallLayerMask()))
            {
                movableDirectionList.Add(pair.Key);
            }
        }
    }

    private Vector3 SelectNextPatrolPoint()
    {
        CheckEightDirectionForPatrol();

        if(movableDirectionList.Count < 0) // 거의 갇힌 상황이면
        {
            enemyStateMachine.ChangeState(enemy.returnState); // 초기 위치로 복귀
            return Vector3.zero; // 더미값 제공. 함수를 끝내기 위함.
        }

        Vector3 MoveToDirection = directionDictionary[movableDirectionList[Random.Range(0, movableDirectionList.Count - 1)]];
        return MoveToDirection;
    }

    private void OnPatrol()
    {
        Vector3 MoveToDirection = SelectNextPatrolPoint();
        ve = MoveToDirection;
        enemy.GetEnemyNavMeshAgent().SetDestination(enemy.transform.position + (MoveToDirection * enemy.f_patrolLength));
    }

    public Vector3 GetPatrolStartPosition() { return startPos; }
}
