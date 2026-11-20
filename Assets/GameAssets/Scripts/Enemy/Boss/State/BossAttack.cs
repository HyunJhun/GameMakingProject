using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossAttack : BossState
{
    public BossAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {
    }
    private bool _isAttack = false;
    private float timeToArrive = 17f;
    public int distanceOfDestination { get; set; } = 17;
    private float timer;
    private float delayTime = 1f;


    public int patternSelectNumber { get; set; } // 0 : BasicAttack , 1: RushAttack 
    // pattern one
    private float pattern_One_Damage = 17f;
    private float pattern_Two_Damage = 10f; // temp Value 
    // pattern two
    public override void Enter()
    {
        initConditions();
        boss.isAttack = true;
        Debug.Log("PATTERN : " + patternSelectNumber);
    }
    public override void Exit()
    {
        //boss.agent.SetDestination(boss.transform.position);
        boss.isAttack = false;
        patternSelectNumber = -1;
    }
    public override void StateActionUpdate()
    {
        if (boss.coolTime_RushAttack <= 3f) boss.coolTime_RushAttack += Time.deltaTime;
        if (boss.coolTime_BreathAttack <= 10f) boss.coolTime_BreathAttack += Time.deltaTime;

        if (_isAttack)
        {
            _isAttack = false;

            CancellationToken ct = boss.GetCancellationTokenOnDestroy();

            if (patternSelectNumber == 0)
                OnBasicAttack(ct).Forget();
            if (patternSelectNumber == 1)
                OnRush(ct).Forget();
            if (patternSelectNumber == 2)
                OnFireBreath(ct).Forget();

            // attack 이 연속적으로 일어나면 일시적으로 attack 상태에서 상태변환이 안일어남
            // 그러면 일단 만약 몇초동안 움직임이 없다면 다시 추격 상태로 강제로 돌려야 하는 방법이 있을수도 있음.
        }
    }
    public override void StateActionFixedUpdate()
    {
        
    }
    private async UniTask OnRush(CancellationToken ct = default)
    {
        await OnDelayBeforeAttack(ct);

        distanceOfDestination = boss.ShotRay(distanceOfDestination);
        boss.BossCollisionBoxActive(true);

        Vector3 attackDirection = (boss.player.transform.position - boss.transform.position).normalized;
        Vector3 destinationOfAttack = new Vector3(
            attackDirection.x * distanceOfDestination,
            0f,
            attackDirection.z * distanceOfDestination) + boss.transform.position;

        boss.transform.LookAt(destinationOfAttack);
        _isAttack = false;
        boss.bossParticleManager.BossAttackParticleInstance(1);
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.RushAttack, false, boss);

        float timer = 0f;
        while (Vector3.Distance(boss.transform.position, destinationOfAttack) > 0.2f)
        {
            timer += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(boss.transform.position, destinationOfAttack, timer / timeToArrive);

            if (boss.CheckPlayerCollisionToBoss(pattern_One_Damage)) break;

            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        boss.BossCollisionBoxActive(false);
        bossStateMachine.ChangeState(boss.stiffnessState);
    }

    private async UniTask OnBasicAttack(CancellationToken ct = default)
    {
        await OnDelayBeforeAttack(ct);

        boss.bossAnimationHandler.OnBasicAttack();

        await UniTask.Delay(TimeSpan.FromSeconds(0.7f), cancellationToken: ct);

        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.BasicAttack, false, boss);
        CheckIsPlayerInRangeOfAttackRangeForBasicAttackPattern();
        bossStateMachine.ChangeState(boss.stiffnessState);
    }

    private async UniTask OnFireBreath(CancellationToken ct = default)
    {
        await OnDelayBeforeAttack(ct);

        boss.transform.LookAt(boss.player.transform);
        boss.bossAnimationHandler.GetBossAnimator().SetTrigger("BreathStart");

        GameObject breathParticle = boss.InstanceFireBraath();
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.BreathAttack, true, boss);
        boss.bossAnimationHandler.GetBossAnimator().SetTrigger("Breathing");

        await UniTask.WaitUntil(
            () => boss.bossAnimationHandler.AnimationPlayingCheck(0, 0.9f, "Breathing"),
            cancellationToken: ct);

        boss.bossAnimationHandler.GetBossAnimator().SetTrigger("BreathEnd");
        SoundManager.soundManagerInstacne.StopSfx(boss);
        GameObject.Destroy(breathParticle, 0.2f);

        await UniTask.WaitUntil(
            () => boss.bossAnimationHandler.AnimationPlayingCheck(0, 0.9f, "Breath End"),
            cancellationToken: ct);

        bossStateMachine.ChangeState(boss.stiffnessState);
    }

    private async UniTask OnDelayBeforeAttack(CancellationToken ct = default)
    {
        if (patternSelectNumber == 0)
        {
            float timer = 0f;
            PooledParticle enterParticle =
                boss.bossParticleManager.BossAttackEnterParticleInstance(0);

            SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.MagicCircle, false, boss);

            while (enterParticle.transform.localScale.x < 6 && enterParticle.transform.localScale.z < 6)
            {
                enterParticle.transform.localScale = Vector3.Lerp(
                    enterParticle.transform.localScale,
                    new Vector3(6f, 0f, 6f),
                    timer / delayTime / 2f);
                timer += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            enterParticle.ReturnToPoolAfter(1f);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: ct);
    }

    private void initConditions()
    {
        _isAttack = true;
        timer = 0f;
        boss.agent.SetDestination(boss.transform.position);
        boss.bossCollisionBox.isCollisionWithPlayer = false;
    }

    private void CheckIsPlayerInRangeOfAttackRangeForBasicAttackPattern()
    {
        if (boss.detectPlayer_AttackRange.getPlayerStatusForDamaged() != null) // 공격 범위 안에 플레이어가 존재할 때
        {
            if (!boss.CheckPlayerDodge())
            {
                boss.detectPlayer_AttackRange.getPlayerStatusForDamaged().hpDown(pattern_Two_Damage - boss.player.GetComponent<Status>().GetArmor());
                boss.player.GetComponent<Player>().b_IsHit = true;
                boss.player.GetComponent<Player>().b_IsKnockback = true;
            }
        }
    }
    
}
