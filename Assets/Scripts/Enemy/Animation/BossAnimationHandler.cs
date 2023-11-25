using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationHandler : MonoBehaviour
{
    private Animator bossAnimator;
    private float speed;
    private void Start()
    {
        bossAnimator = GetComponent<Animator>();
    }

    public void animationUpdate(float speed,bool isDetectPlayer)
    {
        bossAnimator.SetFloat("Speed", speed);
        if(isDetectPlayer)
        {
            bossAnimator.SetTrigger("DetectPlayer");
        }


    }
    // Grounded 관련
    public void OnBasicAttack()
    {
        bossAnimator.SetTrigger("BasicAttack");
    }
    public void OnRushAttack()
    {
        if(bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
            bossAnimator.SetTrigger("Rush");
    }

    // Flight 관련
    public void OnFly(bool isFly)
    {
        bossAnimator.SetBool("OnFly", isFly); // BossFlight 에서 isFly 불값을 얻어옴
    }
    public void OnFloatForAttack()
    {
        bossAnimator.SetTrigger("FloatForAttack");
    }
    public void OnGlideRushAttack()
    {
        bossAnimator.SetTrigger("GlideRush");
    }

    // 공통 Attack
    public void OnFireballAttack()
    {
        bossAnimator.SetTrigger("Fireball");
    }

    // Animation Layer 관련
    public void OnBossAnimationLayerChanger(bool isFly)
    {

        // 0번 레이어의 weight는 0으로 줄일 수가 없으므로 
        if(isFly)
        {
            bossAnimator.SetLayerWeight(1, 1); // Flight Layer Turn On
        }
        else
        {
            bossAnimator.SetLayerWeight(1, 0); // Flight Layer Turn Off
        }
    }

    public Animator GetBossAnimator()
    {
        return bossAnimator;
    }

    
}
