using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : EnemyAttack
{
    public ArcherAttackState(Archer archer, Status status, EnemyStateMachine archerStateMachine) : base(archer, status, archerStateMachine)
    {
    }


    float arrowSpeed = 2f;

    public override void Enter()
    {
        base.Enter();
        enemy.GetAnimator().SetTrigger("Draw");
    }
    public override void StateActionUpdate()
    {
        base.StateActionUpdate();
        enemy.transform.LookAt(enemy.GetPlayer().transform);
        if (animationPlayingCheck("Draw")) enemy.GetAnimator().SetTrigger("OverDraw");
        if (animationPlayingCheck("OverDraw")) enemy.GetAnimator().SetTrigger("Attack");
    }
    public override void Exit()
    {
        base.Exit();
    }

}
