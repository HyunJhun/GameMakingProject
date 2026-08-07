using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
public class BasicAttackPattern : IBossAttackStrategy
{
    public string AttackName => "Basic";
    private float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public async UniTask ExecuteAttack(Boss boss, CancellationToken ct)
    {
        await delayBeforeAttack(boss, ct);
        if (boss == null) return;

        boss.bossAnimationHandler.OnBasicAttack();
        await UniTask.WaitUntil(
            () => boss.bossAnimationHandler.AnimationPlayingCheck(0, 0.7f, "Basic Attack"),
            cancellationToken: ct
            );
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.BasicAttack, false, boss);
        CheckIsPlayerInRangeOfAttackRangeForBasicAttackPattern(boss); // 공격범위 안에 플레이어가 존재하는지 체크.
        //bossStateMachine.ChangeState(boss.stiffnessState);

        // Implement the basic attack logic here
        // For example, you can make the boss perform a simple melee attack
        await UniTask.Yield(); // Ensure this runs on the main thread
    }

    private async UniTask delayBeforeAttack(Boss boss, CancellationToken ct)
    {
        PooledParticle enterParticle =
            boss.bossParticleManager.BossAttackEnterParticleInstance(0);
        ParticleSystem ps = enterParticle.GetComponent<ParticleSystem>();
        float particleDuration = ps.main.duration;

        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.MagicCircle, false, boss);
        
        await enterParticle.transform.DOScale(6f, particleDuration).SetEase(Ease.InOutSine);
        // 수정됨: Destroy 대신 연출 종료 후 풀로 지연 반환
        enterParticle.ReturnToPoolAfter(1f);

        //await UniTask.Delay(System.TimeSpan.FromSeconds(particleDuration), cancellationToken: ct);
    }

    private void CheckIsPlayerInRangeOfAttackRangeForBasicAttackPattern(Boss boss)
    {
        if (boss.detectPlayer_AttackRange.getPlayerStatusForDamaged() != null) // 공격 범위 안에 플레이어가 존재할 때
        {
            if (!boss.CheckPlayerDodge())
            {
                boss.detectPlayer_AttackRange.getPlayerStatusForDamaged().hpDown(damage - boss.player.GetComponent<Status>().GetArmor());
                boss.player.GetComponent<Player>().b_IsHit = true;
                boss.player.GetComponent<Player>().b_IsKnockback = true;
            }
        }
    }
}


//IEnumerator BossAttackPattern_BasicAttack()
//{
//    yield return boss.StartCoroutine(delayBeforeAttack());  // 공격을 하기 전 전조 증상을 플레이어에게 보여주어 플레이어가 대처할 수 있도록 함
//    boss.bossAnimationHandler.OnBasicAttack();
//    yield return new WaitForSeconds(0.7f); // 모션과 타이밍을 맞추기 위해 잠시 시간을 지연
//    SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.BasicAttack, false, boss);
//    CheckIsPlayerInRangeOfAttackRangeForBasicAttackPattern(); // 공격범위 안에 플레이어가 존재하는지 체크.
//    bossStateMachine.ChangeState(boss.stiffnessState);
//}

