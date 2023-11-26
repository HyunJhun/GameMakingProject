using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlight : BossState
{
    public BossFlight(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {

    }
    // property
    private float maxHeightToFly = 2.5f;
    private bool isFly = false;
    

    //reference
    private Rigidbody bossRd;
    public override void Enter()
    {
        phaseTwoInitialize();

    }

    public override void Exit()
    {
        isFly = false;
    }

    public override void StateActionUpdate()
    {
        if(!isFly)
            boss.StartCoroutine(FlyToTheSkyFromGround());
        if(boss.bossAnimationHandler.GetBossAnimator().GetCurrentAnimatorStateInfo(1).IsName("Fly Forward")) // 하늘로 날아 오르는 애니메이션이 끝나고 난 후 상태 변경
        {
            if (boss.bossAnimationHandler.GetBossAnimator().GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f)
                bossStateMachine.ChangeState(boss.flyAroundState);
        }


    }

    public override void StateActionFixedUpdate()
    {
        
    }

    private void phaseTwoInitialize()
    {
        // 페이즈 관련
        boss.isEnterPhaseTwo = true; // 먼저 다시 페이즈 2에 재진입할 일이 없도록 함

        // hp 관련
        stats.SetBossHpToMaxHp(); // 먼저 보스의 hp를 다시 전체로 회복시켜 페이즈 2를 시행한다.    

        // 움직임 관련
        boss.agent.SetDestination(boss.transform.position); // boss.agent.isStopped 를 통해 움직임을 멈춰버리는 것은 단순히 정지시키는 것이라서 별로임.
        boss.agent.enabled = false; // 잠시 공중에서는 길을 찾을 필요도 없고 또한 navmeshagent가 활성화 되어있다면 항상 땅에 붙어있어 공중이동이 불가하므로 false로 변경


        // 레퍼런스
        bossRd = boss.GetComponent<Rigidbody>();
        
    }
    
    IEnumerator FlyToTheSkyFromGround() // 부자연스럽게 날음.. 임시.(암만봐도 lerp 사용해야 할 것 같은데 그게 쉽지가 않네; 왜 정상 적용이 안되는거야
    {
        yield return null;
        float timer = 0f;
        isFly = true;
        bossRd.useGravity = false; // 하늘을 날기 위해 중력은 적용하지 않음.
        boss.bossAnimationHandler.OnBossAnimationLayerChanger(isFly); // 애니메이션 레이어를 변경

        while (boss.transform.position.y < maxHeightToFly)
        {
            timer += Time.deltaTime;
            Vector3 flying = new Vector3(boss.transform.position.x, boss.transform.position.y + 1f, boss.transform.position.z);
            boss.bossAnimationHandler.OnFly(isFly);
            boss.transform.position = Vector3.Lerp(boss.transform.position,flying, timer / 20f); 
            yield return null;
        }
        Debug.Log("공중 종료");
        yield return null;
    }
    
    // y = +2, x.rotation = +28
}

