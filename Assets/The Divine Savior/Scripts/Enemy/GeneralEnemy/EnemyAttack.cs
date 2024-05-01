using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAttack : EnemyState
{
    // Start is called before the first frame update
    public EnemyAttack(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }
    public override void Enter()
    {
        if (!enemy.GetAttackRange().b_isPlayerInRangeOfAttack)
        {
            enemyStateMachine.ChangeState(enemy.chaseState);
            return;
        }
    }
    public override void StateActionUpdate()
    {
        if (animationPlayingCheck("Attack"))
        {
            enemyStateMachine.ChangeState(enemy.chaseState);
            return;
            
        }
    }
    public override void Exit()
    {
        enemy.GetEnemyNavMeshAgent().enabled = true;
    }
    protected bool animationPlayingCheck(string animationName)
    {
        return enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName(animationName) && enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f;
    }
}
