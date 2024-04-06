using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyAttack
{
    public SkeletonAttackState(Enemy skeleton,Status status,EnemyStateMachine skeletonStateMachine) : base(skeleton,status,skeletonStateMachine)
    { 
    }

    public override void Enter()
    {
        enemy.GetAttackRangeBox().SetActive(true);
        enemy.GetAnimator().SetTrigger("Attack");
    }

    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        enemy.GetAttackRangeBox().SetActive(false);
    }
}
