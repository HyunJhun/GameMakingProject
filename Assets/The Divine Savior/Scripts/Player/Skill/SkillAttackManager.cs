using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackManager : MonoBehaviour
{
    [Header("Reference")]
    private Player player;
    private Status playerStatus;
    void Start()
    {
        player = GetComponent<Player>();
        playerStatus = GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwordJudgment()
    {
        playerStatus.MpDown(playerStatus.GetSkillMpUsage(0));


        player.GetPlayerAnimationManager().GetPlayerAnimator().SetTrigger("SwordJudgment");
        player.GetComponent<ParticleManager>().SkillAttackParticleInstance(0);
    }

    public void Heal()
    {
        playerStatus.MpDown(playerStatus.GetSkillMpUsage(1));

        player.GetPlayerAnimationManager().GetPlayerAnimator().SetTrigger("Heal");
        playerStatus.hpIncrease(playerStatus.GetSkillAttackDamage(1));
        player.GetComponent<ParticleManager>().SkillBuffParticleInstance(1);
    }
}
