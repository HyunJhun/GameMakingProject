using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonReadyForAttackState : EnemyReadyForAttack
{
    public SkeletonReadyForAttackState(Enemy skeleton, Status status, EnemyStateMachine skeletonStateMachine) : base(skeleton, status, skeletonStateMachine)
    {
    }

    private float roundMovingTime = 0.0f;
    private float radius;
    private float theta = 0f;
    private Vector3 playerPostion;

    public override void Enter()
    {
        base.Enter();
        onInitialize();
    }

    public override void StateActionUpdate()
    {
        if (currentMovingTime < roundMovingTime)
        {
            onCircularMove();
            currentMovingTime += Time.deltaTime;
            return;
        }

        enemyStateMachine.ChangeState(enemy.attackState);
        return;
    }
    public override void Exit()
    {
        base.Exit();
    }

    private void onInitialize()
    {
        radius = Vector3.Distance(enemy.transform.position, enemy.GetPlayer().transform.position);
        playerPostion = enemy.GetPlayer().transform.position;
        theta = calculateInitTheta();
        roundMovingTime = getRoundMovingTimeRandomly();
    }

    private void onCircularMove()
    {
        float x = playerPostion.x + Mathf.Cos(theta * Mathf.Deg2Rad) * radius;
        float y = enemy.transform.position.y;
        float z = playerPostion.z + Mathf.Sin(theta * Mathf.Deg2Rad) * radius;

        Vector3 enemyNewPosition = new Vector3(x, y, z);

        enemy.transform.position = Vector3.Lerp(enemy.transform.position, enemyNewPosition, Time.deltaTime);
        enemy.transform.LookAt(enemy.GetPlayer().transform);
        theta += enemy.f_attackRoundSpeed * Time.deltaTime * direction;
        if (theta > 360) theta = 0f;
    }
    private float calculateInitTheta()
    {

        Vector2 enemyStartPos = new Vector2(playerPostion.x + radius, playerPostion.z) - new Vector2(playerPostion.x, playerPostion.z);
        Vector2 enemyCurrentPos = new Vector2(enemy.transform.position.x, enemy.transform.position.z) - new Vector2(playerPostion.x, playerPostion.z);
        return Quaternion.FromToRotation(enemyStartPos, enemyCurrentPos).eulerAngles.z;

    }

    private float getRoundMovingTimeRandomly()
    {
        return Random.Range(4, 7);
    }
}
