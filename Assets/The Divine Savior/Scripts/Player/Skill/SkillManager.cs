using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
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
        player.GetParticleManager().SkillAttackParticleInstance(0);
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Judgment, false);
    }

    public void Heal()
    {
        playerStatus.MpDown(playerStatus.GetSkillMpUsage(1));

        player.GetPlayerAnimationManager().GetPlayerAnimator().SetTrigger("Heal");    
        playerStatus.hpIncrease(playerStatus.GetSkillAttackDamage(1));
        player.GetParticleManager().SkillBuffParticleInstance(1);
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Heal, false);
    }
}
