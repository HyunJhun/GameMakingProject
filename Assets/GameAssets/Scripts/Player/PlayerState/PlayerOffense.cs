using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffense : PlayerState
{
    public PlayerOffense(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    int currentAttack = 0;
    float attackCooltime = 0.75f;
    public override void Enter()
    {
        player.GetPlayerAttackCollisionBox().SetActive(true);
        player.b_IsAttack = true;

        OnAttack();
    }
    public override void StateActionUpdate()
    {
        if (attackCooltime > 0) attackCooltime -= Time.deltaTime;

        if (Input.GetButtonDown("Dodge"))
        {
            playerStateMachine.ChangeState(player.dodgeState);
            return;
        }
        if (!player.b_IsAttack)
        {
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                playerStateMachine.ChangeState(player.movingState);
                return;
            }
            if(player.GetKeyInputManager().CheckSkillKeyInput())
            {
                playerStateMachine.ChangeState(player.spellCastingState);
                return;
            }
        }
        if(player.f_PlayerLastAttackTime > 1.0f)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
        if (Input.GetButtonDown("Attack")) // 한번 클릭시 체크가 안되는것은 상태가 변환했기 때문.
        {
            if(attackCooltime <= 0)
                OnAttack();
        }

    }


    public override void Exit()
    {
        player.GetPlayerAttackCollisionBox().SetActive(false);
        player.b_IsAttack = false;
        for(int i = 1; i < 4; i++)
        {
            player.GetPlayerAnimationManager().GetPlayerAnimator().SetBool("Attack" + i, false);
        }
    }

    public void OnAttack()
    {
        // Content

        currentAttack++;
        if (currentAttack > 3) currentAttack = 1;
        // Reset
        if (player.f_PlayerLastAttackTime > 1.0f) currentAttack = 1;

        // Call Triger;
        // Player Context에 저장 (AnimManager가 읽어감)
        //player.currentComboCount = currentAttack;
        //player.b_IsAttack = true;
        //player.f_PlayerLastAttackTime = 0f;

        if (!player.GetPlayerAnimationManager().CheckCurrentAnimationName("Attack1") &&
            !player.GetPlayerAnimationManager().CheckCurrentAnimationName("Attack2") &&
            !player.GetPlayerAnimationManager().CheckCurrentAnimationName("Attack3"))
        {
            player.GetPlayerAnimationManager().GetPlayerAnimator().SetBool("Attack1", true);
        }
        // Reset Timer
        player.f_PlayerLastAttackTime = 0f;

    }

    public void Skill_SwordJudgment()
    {

    }
    // Get Function
    public int GetCurrentAttack() { return currentAttack; }
    public void SetAttackCooltime(float time) { attackCooltime = time; }
}
