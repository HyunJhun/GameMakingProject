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
    public void OnBasicAttack()
    {
        bossAnimator.SetTrigger("BasicAttack");
    }
    public void OnRushAttack()
    {
        if(bossAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
            bossAnimator.SetTrigger("Rush");
    }

    public Animator GetBossAnimator()
    {
        return bossAnimator;
    }
}
