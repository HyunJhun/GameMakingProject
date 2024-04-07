using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherReadyForAttackState : EnemyReadyForAttack
{
    public ArcherReadyForAttackState(Archer archer, Status status, EnemyStateMachine archerStateMachine) : base(archer, status, archerStateMachine)
    {

    }
    float maxStraightMovingTime;
    float moveSpeed;
    Vector3 moveDirection;
    public override void Enter()
    {
        base.Enter();
        onInitialize();
        enemy.GetAnimator().SetInteger("directionSign", (int)direction);
    }
    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
        if(currentMovingTime < maxStraightMovingTime)
        {
            currentMovingTime += Time.deltaTime;
            onStraightMove();
            return;
        }
        enemy.transform.LookAt(enemy.GetPlayer().transform);
        enemyStateMachine.ChangeState(enemy.attackState);
        return;
    }
    public override void Exit()
    {
        base.Exit();
        enemy.GetAnimator().SetInteger("directionSign", 0);
    }

    private void onInitialize()
    {
        maxStraightMovingTime = 2f;
        moveSpeed = 1.5f;
        moveDirection = setMoveDirection();
    }

    private Vector3 setMoveDirection()
    {
        return (direction == 1f) ? Vector3.right : Vector3.right * direction; 
    }

    private void onStraightMove()
    {
        enemy.transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

}
