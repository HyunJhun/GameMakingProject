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
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Enemy.Hit);
        enemy.GetEnemyNavMeshAgent().enabled = false;
        updateEnemyHpUI();

    }

    public override void StateActionUpdate()
    {
        if (enemy.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            enemyStateMachine.ChangeState(enemy.patrolState);
            return;
        }
    }
    public override void Exit()
    {
        enemy.GetEnemyNavMeshAgent().enabled = true;
    }

    private void updateEnemyHpUI()
    {
        if (!enemy.enemyHud.GetHpUIObject().activeSelf)
        {
            enemy.enemyHud.GetHpUIObject().SetActive(true);
            enemy.enemyHud.ResetHpUIAlphaValue();
        }
        else
        {
            enemy.enemyHud.CancelInvoke("FadeOut");
            enemy.enemyHud.StopCoroutine("AlphaFadeOut");
            enemy.enemyHud.ResetHpUIAlphaValue();
            enemy.enemyHud.isFadeOut = false;
        }
    }
}
