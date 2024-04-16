using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : EnemyState
{
    public EnemyGetHit(Enemy enemy, Status stats, EnemyStateMachine enemyStateMachine) : base(enemy, stats, enemyStateMachine)
    { }

    public override void Enter()
    {
        enemy.b_isGetHit = false;
        enemy.GetAnimator().SetTrigger("GetHit");
        enemy.GetEnemyNavMeshAgent().enabled = false;
        updateEnemyHpUI();

    }

    public override void StateActionUpdate()
    {
        //if(enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        //{
        //    enemyStateMachine.ChangeState(enemy.patrolState);
        //    return;
        //}
    }
    public override void Exit()
    {
        enemy.GetEnemyNavMeshAgent().enabled = true;
    }

    private void updateEnemyHpUI()
    {
        if (!enemy.GetComponent<EnemyHUD>().GetHpUIObject().activeSelf)
        {
            enemy.GetComponent<EnemyHUD>().GetHpUIObject().SetActive(true);
            enemy.GetComponent<EnemyHUD>().ResetHpUIAlphaValue();
        }
        else
        {
            enemy.GetComponent<EnemyHUD>().CancelInvoke("FadeOut");
            enemy.GetComponent<EnemyHUD>().StopCoroutine("AlphaFadeOut");
            enemy.GetComponent<EnemyHUD>().ResetHpUIAlphaValue();
            enemy.GetComponent<EnemyHUD>().isFadeOut = false;
        }
    }
}
