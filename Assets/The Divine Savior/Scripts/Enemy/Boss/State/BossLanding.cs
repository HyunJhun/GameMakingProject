using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLanding : BossState
{
    public BossLanding(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {

    }

    private bool isLand;
    private Rigidbody bossRd;
    private float minHeightToLand = 0.2f;
    
    public override void Enter()
    {
        isLand = false;
        bossRd = boss.GetComponent<Rigidbody>();

        if (!isLand)
            boss.StartCoroutine(LandToGroundFromSky());
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void StateActionUpdate()
    {
        if (boss.bossAnimationHandler.GetBossAnimator().GetCurrentAnimatorStateInfo(1).IsName("Land")) // 하늘로 날아 오르는 애니메이션이 끝나고 난 후 상태 변경
        {
            if (boss.bossAnimationHandler.GetBossAnimator().GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f)
            {
                boss.bossAnimationHandler.OnBossAnimationLayerChanger(false);
                bossStateMachine.ChangeState(boss.stiffnessState); // 공중에서 내려온 후 잠시 경직
            }
        }
    }
    IEnumerator LandToGroundFromSky() // 
    {
        yield return null;
        float timer = 0f;
        isLand = true;

        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.FlyUp,boss);
        while (boss.transform.position.y > minHeightToLand)
        {
            timer += Time.deltaTime;
            Vector3 landing = new Vector3(boss.transform.position.x, boss.transform.position.y - 1f, boss.transform.position.z);
            boss.bossAnimationHandler.OnFly(false);
            boss.transform.position = Vector3.Lerp(boss.transform.position, landing, timer / 20f);
            yield return null;
        }
        Debug.Log("공중 종료");
        bossRd.useGravity = true; // 하늘을 날기 위해 중력은 적용하지 않음.
        boss.agent.enabled = true;

        yield return null;
    }
}
