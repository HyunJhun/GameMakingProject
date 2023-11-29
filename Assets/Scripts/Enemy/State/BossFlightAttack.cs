using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlightAttack : BossState
{
    /*
    1. 파이어볼 발사
    2. 날아가면서 불똥 투하(밝거나 시간 지나면 터짐)
    3. 가끔 날아가다가 하강해서 박치기
    4. 잠시 하강해서 휴식(딜타임)
     */
    public int patternSelectNumber { get; set; } = -1; // 0 : 파이어볼 , 1 : 활강 돌진 , 2 : 폭탄 투하
    private float readyTimeToAttack = 3f;
    private bool isFlightAttack;

    // Fireball Attack Property
    public BossFlightAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {

    }
    public override void Enter()
    {
        Debug.Log("패턴 넘버는? : " + patternSelectNumber);
        isFlightAttack = false;
    }

    public override void Exit()
    {
        
    }

    public override void StateActionUpdate()
    {
        if(!isFlightAttack)
            bossPatternCheck_FlightAttack();
    }

    public override void StateActionFixedUpdate()
    {
        
    }

    private void bossPatternCheck_FlightAttack()
    {
        if(patternSelectNumber == 0)
        {
            boss.StartCoroutine(bossFlightAttackPattern_Fireball());
        }
        else if(patternSelectNumber == 1)
        {
            //boss.StartCoroutine(bossFlightAttackPattern_GlideRush());
            Debug.Log("pattern 1");
        }
        else if(patternSelectNumber == 2)
        {
            //boss.StartCoroutine(bossFlightAttackPattern_DiveBomber());
            Debug.Log("pattern 2");
        }
        
    }
    
    IEnumerator bossFlightAttackPattern_Fireball()
    {
        isFlightAttack = true;
        yield return null;
        boss.bossAnimationHandler.OnFireballAttack();
        yield return new WaitForSeconds(2f); // 모션이 충분히 나올 시간을 줌.
        isFlightAttack = false;
        bossStateMachine.ChangeState(boss.flyAroundState);

    }
    IEnumerator bossFlightAttackPattern_GlideRush()
    {
        yield return new WaitForSeconds(readyTimeToAttack);
        Debug.Log("GlideRush");
    }
    IEnumerator bossFlightAttackPattern_DiveBomber()
    {
        yield return new WaitForSeconds(readyTimeToAttack);
        Debug.Log("DiveBomber");
    }

}
